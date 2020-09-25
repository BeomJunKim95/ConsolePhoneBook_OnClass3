using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePhoneBook_OnClass3
{
	public class MenuChoiceException : Exception //사용자정의 예외 클래스는 Exception클래스를 상속해줘야함
												 //상속했을때 빨간줄이 안가는걸 보면 오버라이드를 꼭 해야하는건 아님
	{
		int wrongChoice;

		public MenuChoiceException(string message) : base(message) { }//메세지를 바로 출력할수있게 생성자를 만들어주는게 좋음
																	  // string 인자 하나 주는게 좋음
																	  // Exception 클래스의 message가 읽기전용 속성이라 이렇게 생성자로 바로 보내주는거 밖에
																	  // 방법이없다
		public MenuChoiceException(int choice) : base("다시 메뉴를 선택해주세요")
		{
			wrongChoice = choice;
		}
		public void ShowWrongChoice()
		{
			Console.WriteLine(this.wrongChoice + "에 해당하는 메뉴는 존재하지 않습니다");
		}
	}
}
