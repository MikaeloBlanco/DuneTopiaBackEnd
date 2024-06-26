﻿using Microsoft.EntityFrameworkCore;

namespace DunetopiaBackEnd.Models.Database.Entities;

    [Index(nameof(Id), IsUnique = true)]
    public class Producto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock {  get; set; }
        public string Image {  get; set; }
        public int ProductoCarroId { get; set; }
        public int ProductoPedidoId { get; set; }


    // Foreing Keys
    public ICollection<ProductoCarro> ProductosCarros { get; set; }
    public ICollection<ProductoPedido> ProductosPedidos  { get; set; }
    }

