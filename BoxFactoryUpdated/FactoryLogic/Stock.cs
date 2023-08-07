using BoxFactory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FactoryLogic
{
    public enum Navigate
    {
        AddBox, SearchBox
    }
    public class Stock
    {
        public Queue<ShopItem> shoppingCart = new Queue<ShopItem>();
        public Dictionary<BoxSize, Box> Inventory = new Dictionary<BoxSize, Box>();
        StringBuilder countZeroMsg = new StringBuilder();
        public SortedSet<Box> Boxes { get; set; }
        const double delta = 1.50;
        const int maxCount = 200;
        const int expDays = 14;

        public Stock()
        {
            Boxes = new SortedSet<Box>();
        }

        public Queue<ShopItem> GetValid(string width, string height, string count, Navigate nav)
        {

            double validWidth = Helper.CheckDounle(width);
            double validHeight = Helper.CheckDounle(height);
            int validCount = Helper.Checkint(count);

            if (nav == Navigate.AddBox)
            {
                AddBox(validWidth, validHeight, validCount);
            }
            else if (nav == Navigate.SearchBox)
            {
                SerchBox(validWidth, validHeight, validCount);
            }

            return shoppingCart;
        }

        public void CreateDefault()
        {
            AddBox(20, 10, 20);
            AddBox(30, 30, 15);
            AddBox(20, 13, 40);
            AddBox(50, 10, 10);
            AddBox(15, 60, 20);
            AddBox(30, 60, 20);
            AddBox(15, 32, 20);
            AddBox(70, 10, 15);
            AddBox(25, 10, 50);
            AddBox(15, 25, 30);
            AddBox(40, 20, 25);
            AddBox(12, 30, 35);
            AddBox(30, 40, 18);
            AddBox(20, 20, 50);
            AddBox(25, 15, 45);
            AddBox(35, 25, 30);
            AddBox(18, 12, 60);
            AddBox(45, 20, 20);
            AddBox(10, 50, 25);
            AddBox(50, 30, 15);
            AddBox(30, 35, 22);
            AddBox(20, 45, 18);
            AddBox(15, 15, 40);
            AddBox(40, 25, 20);
            AddBox(12, 60, 15);
            AddBox(60, 20, 12);
            AddBox(25, 30, 30);
            AddBox(35, 18, 25);
            AddBox(20, 50, 18);
            AddBox(50, 10, 35);
            AddBox(30, 22, 30);
            AddBox(15, 18, 50);
            AddBox(45, 20, 25);
            AddBox(10, 25, 45);
            AddBox(50, 30, 20);
            AddBox(30, 35, 15);
            AddBox(18, 40, 22);
            AddBox(25, 15, 30);
            AddBox(35, 20, 40);
            AddBox(20, 12, 35);
        }

        public void AddToJsonFile() => JsonFileSerialize.SimpleWrite(Boxes);

        public async void GetJsonFile()
        {
            try
            {
                var JsonTask = JsonFileSerialize.SimpleRead(); // Deserialization
                var listFromJson = await JsonTask;
                Boxes = listFromJson;
                foreach (var item in Boxes)
                {
                    BoxSize size = new BoxSize(item.BoxSize.Width, item.BoxSize.Height);
                    Inventory[size] = item;
                }
            }
            catch (FileNotFoundException)
            {
                CreateDefault();
            }
        }
        public string CheckDate()
        {
            StringBuilder boxExp = new StringBuilder();

            foreach (Box box in Boxes)
            {
                if (DateTime.Now > box.LastPurshaceDate.AddDays(expDays))
                {
                    boxExp.Append(box.ToString());
                    RemoveBox(box);
                }
            }
            return boxExp.ToString();
        }

        public Queue<ShopItem> SerchBox(double width, double height, int count)
        {
            var boxtofind = new Box(width, height);
            if (Inventory.ContainsKey(boxtofind.BoxSize) && Inventory[boxtofind.BoxSize].Count > count)
            {
                shoppingCart.Enqueue(new ShopItem(Inventory[boxtofind.BoxSize], count));
                return shoppingCart;
            }

            var potentialBoxes = Boxes.GetViewBetween(boxtofind, new Box(width * delta, int.MaxValue));


            foreach (var box in potentialBoxes)
            {
                if (box.BoxSize.Height >= height && box.BoxSize.Height <= height * delta)
                {
                    if (box.Count >= count)
                    {
                        shoppingCart.Enqueue(new ShopItem(box, count));
                        return shoppingCart;
                    }
                    else
                    {
                        shoppingCart.Enqueue(new ShopItem(box, box.Count));
                        count -= box.Count;
                    }

                }

            }
            if (!shoppingCart.Any())
            {
                throw new ArgumentException("boxes in this size range dosent exist");
            }

            return shoppingCart;
        }

        public string CheckOut()
        {

            while (shoppingCart.Count != 0)
            {
                var shopitem = shoppingCart.Dequeue();
                UpdateCounter(shopitem.Box, shopitem.Count);
            }
            return countZeroMsg.ToString();
        }
        public void UpdateCounter(Box boxChange, int count)
        {

            boxChange.Count -= count;
            if (boxChange.Count == 0)
            {
                countZeroMsg.AppendLine(boxChange.ToString());
                RemoveBox(boxChange);
            }
        }

        public void AddBox(double width, double height, int count)
        {
            BoxSize size = new BoxSize(width, height);
            if (!Inventory.ContainsKey(size))
            {
                var newBox = new Box(width, height);
                Boxes.Add(newBox);
                Inventory[size] = newBox;
            }
            if (Inventory[size].Count + count >= maxCount)
            {
                int sendBack = Inventory[size].Count + count - maxCount;
                Inventory[size].Count = maxCount;
                throw new ArgumentException($"box amount is full \nsending back {sendBack}");
            }
            else
                Inventory[size].Count += count;
        }

        public void RemoveBox(Box box)
        {
            Boxes.Remove(box);
            Inventory.Remove(box.BoxSize);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var box in Boxes)
            {
                result.AppendLine($"{box}\n");
            }
            return result.ToString();
        }

    }

}


