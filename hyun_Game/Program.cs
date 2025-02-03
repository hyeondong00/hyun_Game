namespace hyun_Game
{
    class Item
    {
        public string Name { get; }
        public string Description { get; }
        public string Type { get; } // "무기" 또는 "방어구"
        public int Stat { get; } // 공격력 또는 방어력 증가 수치
        public bool IsEquipped { get; set; } // 장착 여부
        public int Price { get; } // 아이템 가격
        public bool IsPurchased { get; set; } // 구매 여부

        public Item(string name, string type, int stat, string description, int price)
        {
            Name = name;
            Type = type;
            Stat = stat;
            Description = description;
            IsEquipped = false;
            IsPurchased = false; // 기본적으로 구매되지않음
            Price = price;
        }
    }

    class Player
    {
        public string Name { get; private set; }
        public string Job { get; private set; } = "전사"; // 기본 직업
        public int Level { get; private set; } = 1; // 기본 레벨 1
        public int BaseAttack { get; private set; } = 10;
        public int BaseDefense { get; private set; } = 5;
        public int HP { get; private set; } = 100;  // 기본 HP 100
        public int Gold { get; private set; } = 1500; // 보유 골드 1500 G

        public List<Item> Inventory { get; private set; }
        private int EquippedAttack => CalculateEquippedStat("무기"); // 장착된 무기의 공격력 합
        private int EquippedDefense => CalculateEquippedStat("방어구"); // 장착된 방어구의 방어력 합

        public Player(string name)
        {
            Name = name;
            Inventory = new List<Item>
            {
                new Item("무쇠갑옷", "방어구", 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1000),
                new Item("스파르타의 창", "무기", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 1500),
                new Item("낡은 검", "무기", 2, "쉽게 볼 수 있는 낡은 검입니다.", 600)
            };
        }

        public void ShowStatus()
        {
            int totalAttack = BaseAttack + EquippedAttack;
            int totalDefense = BaseDefense + EquippedDefense;

            Console.WriteLine("===== 상태창 =====");
            Console.WriteLine($"Lv. {Level:0}");
            Console.WriteLine($"{Name} ({Job})");
            Console.WriteLine($"공격력 : {totalAttack} (+{EquippedAttack})");
            Console.WriteLine($"방어력 : {totalDefense} (+{EquippedDefense})");
            Console.WriteLine($"체  력 : {HP}");
            Console.WriteLine($"Gold : {Gold} G");
            Console.WriteLine("================");

            Console.WriteLine("0. 마을로 돌아가기");
            Console.Write(">> ");
            while (Console.ReadLine() != "0")
            {
                Console.Write("잘못된 입력입니다. 다시 입력하세요: ");
            }
        }

        public void OpenInventory()
        {
            while (true)
            {
                Console.WriteLine("[인벤토리]");
                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    ManageEquipment();
                }
                else if (input == "0")
                {
                    Console.WriteLine("마을로 돌아갑니다.");
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
                }
            }
        }

        private void ManageEquipment()
        {
            while (true)
            {
                Console.WriteLine("\n[아이템 목록]");
                for (int i = 0; i < Inventory.Count; i++)
                {
                    var item = Inventory[i];
                    string equippedMark = item.IsEquipped ? "[E]" : "   ";
                    Console.WriteLine($"- {i + 1} {equippedMark}{item.Name} | {item.Type} +{item.Stat} | {item.Description}");
                }
                Console.WriteLine("0. 나가기");

                Console.Write("원하시는 행동을 입력해주세요: ");
                string input = Console.ReadLine();

                if (input == "0") return;

                if (int.TryParse(input, out int itemIndex) && itemIndex > 0 && itemIndex <= Inventory.Count)
                {
                    ToggleEquip(itemIndex - 1);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
                }
            }
        }

        private void ToggleEquip(int index)
        {
            var item = Inventory[index];
            item.IsEquipped = !item.IsEquipped;
            Console.WriteLine(item.IsEquipped ? $"{item.Name}을(를) 장착했습니다!" : $"{item.Name}을(를) 해제했습니다!");
        }

        private int CalculateEquippedStat(string type)
        {
            int statSum = 0;
            foreach (var item in Inventory)
            {
                if (item.Type == type && item.IsEquipped)
                {
                    statSum += item.Stat;
                }
            }
            return statSum;
        }

        public void Shop()
        {
            List<Item> shopItems = new List<Item>
    {
        new Item("수련자 갑옷", "방어구", 5, "수련에 도움을 주는 갑옷입니다.", 1000),
        new Item("무쇠갑옷", "방어구", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000),
        new Item("스파르타의 갑옷", "방어구", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500),
        new Item("낡은 검", "무기", 2, "쉽게 볼 수 있는 낡은 검입니다.", 600),
        new Item("청동 도끼", "무기", 5, "어디선가 사용됐던 도끼입니다.", 1500),
        new Item("스파르타의 창", "무기", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000)
    };

            while (true)
            {
                Console.WriteLine("[상점] 필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine($"[보유 골드] {Gold} G");

                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < shopItems.Count; i++)
                {
                    var item = shopItems[i];
                    string status;
                    if (item.IsPurchased)
                    {
                        status = "이미 구매한 아이템입니다.";
                    }
                    else if (item.Price <= Gold)
                    {
                        status = $"{item.Price} G";
                    }
                    else
                    {
                        status = $"{item.Price} G (구매불가)";
                    }

                    Console.WriteLine($"{i + 1}. {item.Name} | {item.Type} +{item.Stat} | {item.Description} | {status}");
                }

                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("0. 나가기");

                Console.Write("원하는 행동을 선택하세요: ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    return;
                }
                else if (input == "1")
                {
                    Console.Write("구매할 아이템 번호를 입력하세요: ");
                    if (int.TryParse(Console.ReadLine(), out int itemNumber) && itemNumber > 0 && itemNumber <= shopItems.Count)
                    {
                        BuyItem(shopItems[itemNumber - 1]);
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
                    }
                }
            }
        }

        private void BuyItem(Item item)
        {
            if (item.IsPurchased)
            {
                Console.WriteLine("이 아이템은 이미 구매되었습니다.");
                return;
            }

            if (Gold >= item.Price)
            {
                Gold -= item.Price;
                item.IsPurchased = true; // 아이템 구매 처리

                // 새로운 아이템 객체를 만들어서 인벤토리에 추가
                var purchasedItem = new Item(item.Name, item.Type, item.Stat, item.Description, item.Price);
                Inventory.Add(purchasedItem); // 인벤토리에 아이템 추가
                Console.WriteLine($"{item.Name}을(를) 구매했습니다!");
            }
            else
            {
                Console.WriteLine("골드가 부족합니다.");
            }
        }

    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("나의 첫 게임에 온 걸 환영해!");
            Console.WriteLine("너의 이름을 알려줄래?");
            Console.Write(">> ");
            string nickname = Console.ReadLine();

            Console.WriteLine("환영해, {0}!", nickname);

            Player player = new Player(nickname);

            while (true)
            {
                Console.WriteLine("==== 마을 ====");
                Console.WriteLine("1. 상태창 보기");
                Console.WriteLine("2. 인벤토리 열기");
                Console.WriteLine("3. 상점 방문");
                Console.WriteLine("4. 게임 종료");
                Console.Write("원하는 행동을 선택하세요: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        player.ShowStatus();
                        break;
                    case "2":
                        player.OpenInventory();
                        break;
                    case "3":
                        player.Shop();
                        break;
                    case "4":
                        Console.WriteLine("게임을 종료합니다.");
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 다시 선택하세요.");
                        break;
                }
            }
        }
    }
}