﻿using System.Collections.Generic;

public static class Calendar
{
	public class Month
	{
		public Month(string name, int numDays)
		{
			this.Name = name;
			this.NumDays = numDays;
		}

		public string Name { get; }
		public int NumDays { get; }
	}

	public static List<Month> Months { get; } = new List<Month>
	{
		new Month("Spring", 30),
		new Month("Summer", 30),
		new Month("Fall", 30),
		new Month("Winter", 30)
	};

	public static int DaysInYear {
		get
		{
			int result = 0;
			foreach (Month m in Months)
			{
				result += m.NumDays;
			}
			return result;
		} 
	}

	public static int GetDayOfMonth (int dayOfYear)
	{
		int remaining = dayOfYear;
		while (true)
		{
			foreach (Month m in Months)
			{
				if (remaining > m.NumDays)
				{
					remaining -= m.NumDays;
				}
				else
				{
					return remaining;
				}
			}
		}
	}

	// not zero-indexed
	public static Month GetMonth(int dayOfYear)
	{
		int remaining = dayOfYear;
		while (true)
		{
			foreach (Month m in Months)
			{
				if (remaining > m.NumDays)
				{
					remaining -= m.NumDays;
				}
				else
				{
					return m;
				}
			}
		}
	}
	public static Month GetFollowingMonth (Month month)
	{
		int index = -1;
		for (int i = 0; i < Months.Count; i++)
		{
			if (Months[i].Equals(month))
				index = i;
		}
		index++;
		index %= Months.Count;
		return Months[index];
	}
}
