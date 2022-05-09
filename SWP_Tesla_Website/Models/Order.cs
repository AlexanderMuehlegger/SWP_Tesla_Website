namespace SWP_Tesla_Website.Models {
    public class Order {
        public int ID { get; set; }
        public int user_id { get; set; }
        public int article_id { get; set; }
        public decimal Saldo { get; set; }
    }

    public enum OrderStatus {
        open, paid, cancelled
    }
}
