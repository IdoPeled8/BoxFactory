namespace FactoryLogic
{
    public class ShopItem
    {
        public Box Box { get; }
        public int Count { get; }
        public ShopItem(Box box, int count)
        {
            Box = box;
            Count = count;
        }

        public override string ToString()
        {
            return $"i can give you this box parameters:\n{Box}\n with the amount of:{Count}";
        }
    }
    

}

