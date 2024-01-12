using System.Text.Json.Serialization;
using SportsStore.Infrastructure;
namespace SportsStore.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services) // a factory for creating SessionCart objects and providing them with an ISession object
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session?.SetJson("Cart", this); // store the updated state in the session using the extnesion methods on the ISession interface
        }
        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session?.SetJson("Cart", this);
        }
        public override void Clear()
        {
            base.Clear();
            Session?.Remove("Cart");
        }

    }
}