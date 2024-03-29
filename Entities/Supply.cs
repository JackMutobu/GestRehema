﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestRehema.Entities
{
    public class Supply:IBaseEntity
    {
        public Supply()
        {
            Expenses = new Collection<SupplyExpense>();
            SupplyItems = new Collection<SupplyItem>();
        }
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime DateOperation { get; set; }

        public string ShortDateOperation => DateOperation.ToShortDateString();

        public string ShortUpdateAt => UpdatedAt.GetValueOrDefault().ToShortDateString();

        public string PayementStatus { get; set; } = SupplyPayementStatus.AwaitingPayement;

        public string DeliveryStatus { get; set; } = SupplyDeliveryStatus.AwaitingDelivery;

        public ICollection<SupplyExpense> Expenses { get; set; }

        public ICollection<SupplyItem> SupplyItems { get; set; }
    }

}
