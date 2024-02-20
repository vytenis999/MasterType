using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(StoreContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "Test",
                    Email = "test@test.com"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");

                var admin = new User
                {
                    UserName = "TestAdmin",
                    Email = "admintest@test.com"
                };

                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] {"Member", "Admin"});
            }

            if (context.Products.Any()) return;

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Hyperx Alloy Origins 60 Red switches",
                    Description =
                        "The HyperX Alloy Origins™ 60 is a supremely portable 60% form factor keyboard that gives you more room for sweeping mouse movements. It features a durable, full aluminum body, and reliable HyperX switches balanced for speed and performance which are rated to withstand 80 million keypresses. The stock double shot PBT keycaps have secondary functions printed on them so you can quickly locate additional functionality. Let your style shine bright with the HyperX switches’ exposed LED design, and stunning lighting effects. Further customize your keyboard with macros, personalized lighting profiles, and more with HyperX NGENUITY software.",
                    Price = 8900,
                    PictureUrl = "/images/products/hyperx_alloy_origins_60_1.png",
                    Brand = "HyperX",
                    Type = "Red switches",
                    QuantityInStock = 3
                },
                new Product
                {
                    Name = "Corsair Gaming K63 Red switches",
                    Description = "Experience ultimate gaming freedom with the CORSAIR K63 Wireless Mechanical Gaming Keyboard, featuring ultra-fast 1ms 2.4GHz wireless technology with CHERRY® MX mechanical keyswitches packed into a portable, tenkeyless design.",
                    Price = 7900,
                    PictureUrl = "/images/products/corsair_gaming_k63_1.png",
                    Brand = "Corsair Gaming",
                    Type = "Red switches",
                    QuantityInStock = 4
                },
                new Product
                {
                    Name = "Steelseries Apex 7 RGB Red switches",
                    Description = "The SteelSeries Apex 7 TKL is gaming keyboard with tenkeyless layouts. The SteelSeries is a wired-only model with a more conventional look and full RGB backlighting. It comes with your choice of linear, tactile, or clicky mechanical switches.",
                    Price = 15000,
                    PictureUrl = "/images/products/steelseries_apex_7_1.png",
                    Brand = "Steelseries",
                    Type = "Red switches",
                    QuantityInStock = 1
                },
                new Product
                {
                    Name = "Ducky one 2 mini RGB Red switches",
                    Description = "Compact ten keyless mini design in black. High quality Cherry Mx Red key switch. Stylish RGB LED backlight. PBT double shot key caps for long life.",
                    Price = 10499,
                    PictureUrl = "/images/products/ducky_one_2_mini_rgb_black_1.png",
                    Brand = "Ducky",
                    Type = "Red switches",
                    QuantityInStock = 6
                },

                new Product
                {
                    Name = "Corsair Gaming K95 RGB Blue switches",
                    Description = "The Corsair Gaming K95 RGB mechanical gaming keyboard begins with the performance of the legendary K95, and adds multicolor per-key backlighting for virtually unlimited customization. Every key is backed with a CHERRY® MX RGB key switch for precise actuation and superior feel.",
                    Price = 19999,
                    PictureUrl = "/images/products/corsair_gaming_k95_rgb_1.png",
                    Brand = "Corsair Gaming",
                    Type = "Blue switches",
                    QuantityInStock = 8
                },
                new Product
                {
                    Name = "Ducky One 2 SF RGB Blue switches",
                    Description = "The function package is packed in a sleek black and white case without numeric keypad, with a very narrow frame and RGB LED backlight. The keyboard with Cherry MX Blue switches is a compromise between a pure gaming keyboard and an ideal typewriter keyboard.",
                    Price = 12999,
                    PictureUrl = "/images/products/ducky_one_2_sf_rgb.png",
                    Brand = "Ducky",
                    Type = "Blue switches",
                    QuantityInStock = 3
                },
                new Product
                {
                    Name = "Steelseries Apex 7 RGB Blue switches",
                    Description = "The SteelSeries Apex 7 TKL is gaming keyboard with tenkeyless layouts. The SteelSeries is a wired-only model with a more conventional look and full RGB backlighting. It comes with your choice of linear, tactile, or clicky mechanical switches.",
                    Price = 18999,
                    PictureUrl = "/images/products/steelseries_apex_7_1.png",
                    Brand = "Steelseries",
                    Type = "Blue switches",
                    QuantityInStock = 5
                },
                new Product
                {
                    Name = "Genesis Thor 420 RGB Blue switches",
                    Description = "The awesome slim mechanical keyboard. Constructed for the most demanding players appreciative not only advanced functionality, but also beautiful, luxurious design. Made of shiny aluminum tempts with its appearance, content with the possibilities and viability switches to 50 million clicks.",
                    Price = 8499,
                    PictureUrl = "/images/products/genesis_thor_420_1.png",
                    Brand = "Genesis",
                    Type = "Blue switches",
                    QuantityInStock = 3
                },
                new Product
                {
                    Name = "Deltaco DK230 TKL Membrane",
                    Description = "This membrane RGB keyboard takes up little space on your desk, the knob in the top right corner has a multi-purpose allowing you to adjust both volume and the lighting. There is no software required to control the RGB of this keyboard.",
                    Price = 2999,
                    PictureUrl = "/images/products/deltaco_dk230_TKL_1.png",
                    Brand = "Deltaco",
                    Type = "Membrane",
                    QuantityInStock = 9
                },
                new Product
                {
                    Name = "HyperX Alloy Core RGB Membrane",
                    Description = "Crafted with a durable, reinforced plastic frame, the Alloy Core RGB was constructed for stability and reliability for gamers who want a keyboard that will last. The soft-touch keys have a tactile feel, yet are tuned to be quiet, and they also feature gaming-grade anti-ghosting functionality and key rollover.",
                    Price = 5999,
                    PictureUrl = "/images/products/hyperx_alloy_core_rgb_1.png",
                    Brand = "HyperX",
                    Type = "Membrane",
                    QuantityInStock = 6
                },
                new Product
                {
                    Name = "Logitech G213 Prodigy RGB Membrane",
                    Description = "The G213 gaming keyboard features Logitech G Mech-Dome keys that are specially tuned to deliver a superior tactile response and overall performance profile similar to a mechanical keyboard. Mech-Dome keys are full height, deliver a full 4mm travel distance, 50g actuation force, and a quiet sound operation.",
                    Price = 4899,
                    PictureUrl = "/images/products/logitech_g213_prodigy_1.png",
                    Brand = "Logitech",
                    Type = "Membrane",
                    QuantityInStock = 11
                },
                new Product
                {
                    Name = "Razer Ornata Chroma v2 RGB Membrane",
                    Description = "Returning with Razer Hybrid Mecha-Membrane Technology, this keyboard merges the benefits of membrane keys and mechanical switchesfor the best of both worlds. Powered by Razer Chroma RGB for customizable style and upgraded with a Multi-function Digital Wheel and Media Keys for even more control.",
                    Price = 5999,
                    PictureUrl = "/images/products/razer_ornata_v2_1.png",
                    Brand = "Razer",
                    Type = "Membrane",
                    QuantityInStock = 1
                },
            };

            context.Products.AddRange(products);    

            context.SaveChanges();
        }
    }
}
