﻿using ActorComponents;

public class TradeSystem
{
	// The multiplier by item value for a customer selling items
	private const float SellPriceMultiplier = 0.9f;

	// Performs the given trade, exchanging items and money. Assumes that all items with a given ID are identical.
	public static bool ActivateTrade (TradeTransaction trade)
	{
		if (!CustomerHasSufficientFunds(trade) || !VendorHasSufficientFunds(trade))
		{
			return false;
		}
		ActorData vendor = ActorRegistry.Get(trade.vendorActorId).data;
		ActorData customer = ActorRegistry.Get(trade.customerActorId).data;

		vendor.Wallet.AddBalance(-trade.TransactionTotal);
		customer.Wallet.AddBalance(trade.TransactionTotal);

		foreach (string itemId in trade.itemPurchases.Keys)
		{
			vendor.Inventory.Remove(itemId, trade.itemPurchases[itemId]);
			for (int i = 0; i < trade.itemPurchases[itemId]; i++)
			{
				customer.Inventory.AttemptAddItem(new ItemStack(itemId, 1));
			}
		}
		foreach (string itemId in trade.itemSells.Keys)
		{
			customer.Inventory.Remove(itemId, trade.itemSells[itemId]);
			for (int i = 0; i < trade.itemSells[itemId]; i++)
			{
				vendor.Inventory.AttemptAddItem(new ItemStack(itemId, 1));
			}
		}
		return true;
	}

	public static bool CustomerHasSufficientFunds (TradeTransaction trade)
	{
		return (ActorRegistry.Get(trade.customerActorId).data.Wallet.Balance >= -trade.TransactionTotal);
	}

	public static bool VendorHasSufficientFunds(TradeTransaction trade)
	{
		return (ActorRegistry.Get(trade.vendorActorId).data.Wallet.Balance >= trade.TransactionTotal);
	}

	public static int GetItemPurchasePrice(string itemId, string buyerActorId, string vendorActorId)
	{
		ActorData vendor = ActorRegistry.Get(vendorActorId).data;
		Trader trader = vendor.GetComponent<Trader>();

		return trader.GetPurchasePrice(itemId);
	}

	public static int GetItemSellPrice (string itemId, string buyerActorId, string vendorActorId)
	{
		ItemData item = ContentLibrary.Instance.Items.Get(itemId);
		// Reduce the price when the customer sells items (so the vendor can make a living)
		return (int)(item.BaseValue * SellPriceMultiplier);
	}
}
