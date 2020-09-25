using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePhoneBook_OnClass3
{
	public class PhoneBookManager
	{
		const int MAX_CNT = 100;
		PhoneInfo[] infoStorage = new PhoneInfo[MAX_CNT];
		int curCnt = 0;

		public void ShowMenu()
		{
			Console.WriteLine("----------------------------- 주소록 -----------------------------");
			Console.WriteLine("1. 입력  |  2. 목록  |  3. 검색  |  4. 정렬  |  5. 삭제  |  6. 종료");
			Console.WriteLine("-----------------------------------------------------------------");
			Console.Write("선택: ");
		}

		public void SortData()
		{
			
			int choice;
			while (true)
			{
				try
				{
					Console.WriteLine("1. 이름 오름차순  |  2. 이름 내림차순  |  3. 전화번호 오름차순  |  4. 전화번호 내림차순");
					Console.Write("선택 >> ");
					if (int.TryParse(Console.ReadLine(), out choice))
					{
						if (choice < 1 || choice > 4)
						{
							throw new MenuChoiceException(choice);
							//Console.WriteLine("1~4 까지의 숫자만 입력해주세요.");
							//return;
						}
						else
							break;
					}

				}
				catch(MenuChoiceException err)
				{
					err.ShowWrongChoice();
				}
			}


			PhoneInfo[] new_arr = new PhoneInfo[curCnt]; // 배열복제는 다 해야하니 밖으로 뺴서 한번만
			Array.Copy(infoStorage, new_arr, curCnt);

			if (choice == 1)
			{
				Array.Sort(new_arr); //인포스토리지를 바로 줘버리면 원본이 바뀌어버리니 카피떠서 해야함 그래서 카피한 배열을 줌
									 //내가 만든 배열이기 때문에 컴파일러가 정렬할 기준을 몰라 내가 기준도 정해줘야함
			}
			else if (choice == 2)
			{
				Array.Sort(new_arr);
				Array.Reverse(new_arr); //reverse만하면 오름차순으로 정렬한 배열에 내림차순 완성
			}
			else if (choice == 3)
			{
				Array.Sort(new_arr, new PhoneComparator()); // new PhoneComparator() 객체하나 만들어서 넘겨주면됨
			}
			else if (choice == 4)
			{
				Array.Sort(new_arr, new PhoneComparator());
				Array.Reverse(new_arr);
			}

			for (int i = 0; i < curCnt; i++) // 출력도 마지막에 한번만 하면 되니 따로 뺴고
			{
				new_arr[i].ShowPhoneInfo();
				Console.WriteLine();
			}
		}

		public void InputData()
		{
			Console.WriteLine("1.일반  2.대학  3.회사");
			Console.Write("선택 >> ");
			int choice;
			while (true)
			{
				if (int.TryParse(Console.ReadLine(), out choice))
					break;
			}
			if (choice < 1 || choice > 3)
			{
				Console.WriteLine("1.일반  2.대학  3.회사 중에 선택하십시오.");
				return;
			}
			PhoneInfo info = null;
			switch (choice)
			{
				case 1:
					info = InputFriendInfo();
					break;
				case 2:
					info = InputUnivInfo();
					break;
				case 3:
					info = InputCompanyInfo();
					break;
			}
			if (info != null)
			{
				infoStorage[curCnt++] = info;
				Console.WriteLine("데이터 입력이 완료되었습니다");
			}
		}

		//반복되는 코딩을 줄이고 구조화된 코드를 만들수 있다 
		//기능별로 메서드를 쪼개서 코드의 가독성을 늘일수 있다
		private string[] InputCommonInfo() //리턴을 여러개를 받을수 없으니 배운 배열을 사용해 여러개의 리턴을 받자
										   //반환타입을 배열로 선언
		{
			Console.Write("이름: ");
			string name = Console.ReadLine().Trim();
			//if (name == "") or if (name.Length < 1) or if (name.Equals(""))
			if (string.IsNullOrEmpty(name))
			{
				Console.WriteLine("이름은 필수입력입니다");
				return null;  //메서드의 타입이 PhoneBookManager 즉 클래스기 때문에 클래스는 레퍼런스타입이라 return값을 null로 줘야함
			}
			else
			{
				int dataIdx = SearchName(name);
				if (dataIdx > -1)
				{
					Console.WriteLine("이미 등록된 이름입니다. 다른 이름으로 입력하세요");
					return null;
				}
			}

			Console.Write("전화번호: ");
			string phone = Console.ReadLine().Trim();
			if (string.IsNullOrEmpty(phone))
			{
				Console.WriteLine("전화번호는 필수입력입니다");
				return null;
			}

			Console.Write("생일: ");
			string birth = Console.ReadLine().Trim();

			string[] arr = new string[3];
			arr[0] = name;
			arr[1] = phone;
			arr[2] = birth;

			return arr;
		}

		private PhoneInfo InputFriendInfo()
		{
			string[] comInfo = InputCommonInfo();
			if (comInfo == null || comInfo.Length != 3)
				return null;

			if (comInfo[2].Length < 1)
				return new PhoneInfo(comInfo[0], comInfo[1]);
			else
				return new PhoneInfo(comInfo[0], comInfo[1], comInfo[2]);
		}

		private PhoneInfo InputUnivInfo()
		{
			string[] comInfo = InputCommonInfo();
			if (comInfo == null || comInfo.Length != 3)
				return null;

			Console.Write("전공: ");
			string major = Console.ReadLine().Trim();

			Console.Write("학년: ");
			int year = int.Parse(Console.ReadLine().Trim());

			return new PhoneUnivInfo(comInfo[0], comInfo[1], comInfo[2], major, year);
		}

		private PhoneInfo InputCompanyInfo()
		{
			string[] comInfo = InputCommonInfo();
			if (comInfo == null || comInfo.Length != 3)
				return null;

			Console.Write("회사명: ");
			string company = Console.ReadLine().Trim();



			return new PhoneCompanyInfo(comInfo[0], comInfo[1], comInfo[2], company);
		}

		public void ListData()
		{
			if (curCnt == 0)
			{
				Console.WriteLine("입력된 데이터가 없습니다.");
				return;
			}

			for (int i = 0; i < curCnt; i++)
			{
				infoStorage[i].ShowPhoneInfo();
				Console.WriteLine();
			}
		}

		public void SearchData()
		{
			Console.WriteLine("주소록 검색을 시작합니다......");
			int dataIdx = SearchName();
			if (dataIdx < 0)
			{
				Console.WriteLine("검색된 데이터가 없습니다");
			}
			else
			{
				infoStorage[dataIdx].ShowPhoneInfo();
			}

			#region 모두 찾기
			//int findCnt = 0;
			//for(int i=0; i<curCnt; i++)
			//{
			//    // ==, Equals(), CompareTo()
			//    if (infoStorage[i].Name.Replace(" ","").CompareTo(name) == 0)
			//    {
			//        infoStorage[i].ShowPhoneInfo();
			//        findCnt++;
			//    }
			//}
			//if (findCnt < 1)
			//{
			//    Console.WriteLine("검색된 데이터가 없습니다");
			//}
			//else
			//{
			//    Console.WriteLine($"총 {findCnt} 명이 검색되었습니다.");
			//}
			#endregion
		}

		private int SearchName()
		{
			Console.Write("이름: ");
			string name = Console.ReadLine().Trim().Replace(" ", "");

			for (int i = 0; i < curCnt; i++)
			{
				if (infoStorage[i].Name.Replace(" ", "").CompareTo(name) == 0)
				{
					return i;
				}
			}

			return -1;
		}

		private int SearchName(string name)
		{
			for (int i = 0; i < curCnt; i++)
			{
				if (infoStorage[i].Name.Replace(" ", "").CompareTo(name) == 0)
				{
					return i;
				}
			}

			return -1;
		}

		public void DeleteData()
		{
			Console.WriteLine("주소록 삭제를 시작합니다......");

			int dataIdx = SearchName();
			if (dataIdx < 0)
			{
				Console.WriteLine("삭제할 데이터가 없습니다");
			}
			else
			{
				for (int i = dataIdx; i < curCnt; i++)
				{
					infoStorage[i] = infoStorage[i + 1];
				}
				curCnt--;
				Console.WriteLine("주소록 삭제가 완료되었습니다");
			}
		}
	}
}
