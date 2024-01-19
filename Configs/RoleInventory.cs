namespace SpiesSCP.Configs
{
    using System;
    using System.Collections.Generic;

    using ConfigObjects;
    using YamlDotNet.Serialization;

    public class RoleInventory
    {
        [YamlIgnore]
        public int UsedSlots
        {
            get
            {
                int i = 0;
                if (Slot3 != null && !Slot3.IsEmpty())
                    i++;
                if (Slot4 != null && !Slot4.IsEmpty())
                    i++;
                if (Slot5 != null && !Slot5.IsEmpty())
                    i++;
                if (Slot6 != null && !Slot6.IsEmpty())
                    i++;
                if (Slot7 != null && !Slot7.IsEmpty())
                    i++;
                if (Slot8 != null && !Slot8.IsEmpty())
                    i++;
                return i;
            }
        }

        public List<ItemChance> Slot3 { get; set; } = new();

        public List<ItemChance> Slot4 { get; set; } = new();

        public List<ItemChance> Slot5 { get; set; } = new();

        public List<ItemChance> Slot6 { get; set; } = new();

        public List<ItemChance> Slot7 { get; set; } = new();

        public List<ItemChance> Slot8 { get; set; } = new();

        public IEnumerable<ItemChance> this[int i] => i switch
        {
            0 => Slot3,
            1 => Slot4,
            2 => Slot5,
            3 => Slot6,
            4 => Slot7,
            5 => Slot8,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}