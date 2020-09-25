using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePhoneBook_OnClass3
{
	class PhoneComparator : IComparer

	{
		public int Compare(object x, object y)
		{
			PhoneInfo first = (PhoneInfo)x;
			PhoneInfo other = (PhoneInfo)y;
			//if (this.name.CompareTo(other.name) == 1) //문자열은 크다작다 연산자가 없음,  앞에게 더크면 1
			//    return 1;
			//else if (this.name.CompareTo(other.name) == -1)
			//    return -1;
			//else
			//    return 0;

			return first.Phone.CompareTo(other.Phone); // 한줄
		}
	}
}
