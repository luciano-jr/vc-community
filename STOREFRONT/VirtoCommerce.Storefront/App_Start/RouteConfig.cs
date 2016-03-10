using FluentRouting.Mvc;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Routing;
using VirtoCommerce.Storefront.Controllers.Api;
using VirtoCommerce.Storefront.Controllers;
using System.Net.Http;

namespace VirtoCommerce.Storefront
{
    public class RouteConfig
    {
        
        public static void RegisterRoutes(RouteCollection routes, Func<WorkContext> workContextFactory, ICommerceCoreModuleApi commerceCoreApi, IStaticContentService staticContentService, ICacheManager<object> cacheManager)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //We disable simultanous using Convention and Attribute routing because we have SEO slug urls and optional Store and Languages url parts 
            //this leads to different kinds of collisions and we deside to use only Convention routing 

            //routes.MapMvcAttributeRoutes();

            #pragma warning disable 4014

            routes.For<ApiCartController>()
                .CreateRoute("{store?}/{langauge?}/storefrontapi/cart").To(c => c.GetCart())
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/itemscount").To(c => c.GetCartItemsCount())
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/items").To(c => c.AddItemToCart(null, 1)).WithConstraints().HttpMethod(HttpMethod.Post)
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/items").To(c => c.ChangeCartItem(null, 1)).WithConstraints().HttpMethod(HttpMethod.Put)
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/items").To(c => c.RemoveCartItem(null)).WithConstraints().HttpMethod(HttpMethod.Delete)
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/clear").To(c => c.ClearCart())
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/shipments/{shipmentId}/shippingmethods").To(c => c.GetCartShipmentAvailShippingMethods(null))
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/paymentmethods").To(c => c.GetCartAvailPaymentMethods())
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/coupons/{couponCode}").To(c => c.AddCartCoupon(null))
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/coupons").To(c => c.RemoveCartCoupon())
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/shipments").To(c => c.AddOrUpdateCartShipment(null))
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/payments").To(c => c.AddOrUpdateCartPayment(null))
                .CreateRoute("{store?}/{language?}/storefrontapi/cart/createorder").To(c => c.CreateOrder(null));

            routes.For<ApiCatalogController>()
                .CreateRoute("{store?}/{language?}/storefrontapi/catalog/search").To(c => c.SearchProducts(null)).WithConstraints().HttpMethod(HttpMethod.Post)
                .CreateRoute("{store?}/{language?}/storefrontapi/products").To(c => c.GetProductsByIds(null, Model.Catalog.ItemResponseGroup.ItemLarge))
                .CreateRoute("{store?}/{language?}/storefrontapi/categories").To(c => c.GetCategoriesByIds(null, Model.Catalog.CategoryResponseGroup.Full));

            routes.For<ApiCommonController>()
                .CreateRoute("{store?}/{language?}/storefrontapi/countries").To(c => c.GetCountries())
                .CreateRoute("{store?}/{language?}/storefrontapi/countries/{countryCode}/regions").To(c => c.GetCountryRegions(null));

            routes.For<ApiPricingController>()
                .CreateRoute("{store?}/{language?}/storefrontapi/pricing/actualprices").To(c => c.GetActualProductPrices(null));

            routes.For<ApiMarketingController>()
                .CreateRoute("{store?}/{language?}/storefrontapi/marketing/dynamiccontent/{placeName}").To(c => c.GetDynamicContent(null));

            routes.For<ApiAccountController>()
                .CreateRoute("{store?}/{language?}/storefrontapi/account").To(c => c.GetCurrentCustomer());

            routes.For<ApiQuoteRequestController>()
                .CreateRoute("{store?}/{language?}/storefrontapi/quoterequests/{number}/itemscount").To(c => c.GetItemsCount(null))
                .CreateRoute("{store?}/{language?}/storefrontapi/quoterequests/{number}").To(c => c.Get(null))
                .CreateRoute("{store?}/{language?}/storefrontapi/quoterequest/current").To(c => c.GetCurrent())
                .CreateRoute("{store?}/{language?}/storefrontapi/quoterequests/current/items").To(c => c.AddItem(null, 1))
                .CreateRoute("{store?}/{language?}/storefrontapi/quoterequests/{number}/items/{itemId}").To(c => c.RemoveItem(null, null))
                .CreateRoute("{store?}/{language?}/storefrontapi/quoterequests/{number}").To(c => c.Update(null, null))
                .CreateRoute("{store?}/{language?}/storefrontapi/quoterequests/{number}/submit").To(c => c.Submit(null, null))
                .CreateRoute("{store?}/{language?}/storefrontapi/quoterequests/{number}/reject").To(c => c.Reject(null))
                .CreateRoute("{store?}/{language?}/storefrontapi/quoterequests/{number}/totals").To(c => c.CalculateTotals(null, null))
                .CreateRoute("{store?}/{language?}/storefrontapi/quoterequests/{number}/confirm").To(c => c.Confirm(null, null));

            routes.For<AccountController>()
                .CreateRoute("{store?}/{language?}/account").To(c => c.GetAccount()).WithConstraints().HttpMethod(HttpMethod.Get)
                .CreateRoute("{store?}/{language?}/account").To(c => c.UpdateAccount(null)).WithConstraints().HttpMethod(HttpMethod.Post)
                .CreateRoute("{store?}/{language?}/account/order/{number}").To(c => c.GetOrderDetails(null))
                .CreateRoute("{store?}/{language?}/account/addresses/{id}").To(c => c.UpdateAddress(null, null))
                .CreateRoute("{store?}/{language?}/account/addresses").To(c => c.GetAddresses())
                .CreateRoute("{store?}/{language?}/account/register").To(c => c.Register())
                .CreateRoute("{store?}/{language?}/account/login").To(c => c.Login(null))
                .CreateRoute("{store?}/{language?}/account/logout").To(c => c.Logout())
                .CreateRoute("{store?}/{language?}/account/forgotpassword").To(c => c.ForgotPassword(null))
                .CreateRoute("{store?}/{language?}/account/resetpassword").To(c => c.ResetPassword(null))
                .CreateRoute("{store?}/{language?}/account/password").To(c => c.ChangePassword(null));

            routes.For<CartController>()
                .CreateRoute("{store?}/{language?}/cart").To(c => c.Index()).WithConstraints().HttpMethod(HttpMethod.Get)
                .CreateRoute("{store?}/{language?}/cart/checkout").To(c => c.Checkout())
                .CreateRoute("{store?}/{language?}/cart/externalpaymentcallback").To(c => c.ExternalPaymentCallback())
                .CreateRoute("{store?}/{language?}/cart/thanks/{orderNumber}").To(c => c.Thanks(null))
                .CreateRoute("{store?}/{languahe?}/cart/checkout/paymentform").To(c => c.PaymentForm(null));

            routes.For<ShopifyCompatibilityController>()
                .CreateRoute("{store?}/{language?}/cart").To(c => c.Cart(null, null)).WithConstraints().HttpMethod(HttpMethod.Post)
                .CreateRoute("{store?}/{language?}/cart.js").To(c => c.CartJs())
                .CreateRoute("{store?}/{language?}/cart/add").To(c => c.Add(null, 1))
                .CreateRoute("{store?}/{language?}/cart/add.js").To(c => c.AddJs(null, 1))
                .CreateRoute("{store?}/{language?}/cart/change").To(c => c.Change(0, 0))
                .CreateRoute("{store?}/{language?}/cart/change.js").To(c => c.ChangeJs(null, 0))
                .CreateRoute("{store?}/{language?}/cart/clear").To(c => c.Clear()).WithConstraints().HttpMethod(HttpMethod.Get)
                .CreateRoute("{store?}/{language?}/cart/clear.js").To(c => c.ClearJs())
                .CreateRoute("{store?}/{language?}/cart/update.js").To(c => c.UpdateJs(null));

            routes.For<QuoteRequestController>()
                .CreateRoute("{store?}/{language?}/quoterequest").To(c => c.CurrentQuoteRequest())
                .CreateRoute("{store?}/{language?}/account/quoterequests").To(c => c.QuoteRequests())
                .CreateRoute("{store?}/{language?}/quoterequest/{number}").To(c => c.QuoteRequestByNumber(null));

            routes.For<CatalogSearchController>()
                .CreateRoute("{store?}/{language?}/search/{categoryId}").To(c => c.CategoryBrowsing(null, null))
                .CreateRoute("{store?}/{language?}/search").To(c => c.SearchProducts());

            routes.For<CommonController>()
                .CreateRoute("{store?}/{language?}/common/setcurrency/{currency}").To(c => c.SetCurrency("USD", null))
                .CreateRoute("{store?}/{language?}/contact/{viewName}").To(c => c.СontactUs("page.contact")).WithConstraints().HttpMethod(HttpMethod.Get)
                .CreateRoute("{store?}/{language?}/contact/{viewName}").To(c => c.СontactUs(null, "page.contact")).WithConstraints().HttpMethod(HttpMethod.Post)
                .CreateRoute("{store?}/{language?}/common/nostore").To(c => c.NoStore())
                .CreateRoute("{store?}/{language?}/maintenance").To(c => c.Maintenance());

            routes.For<ProductController>()
                .CreateRoute("{store?}/{language?}/product/{productId}").To(c => c.ProductDetails(null));

            routes.For<AssetController>()
                .CreateRoute("{store?}/{language?}/themes/assets/{asset}").To(c => c.GetAssets(null))
                .CreateRoute("{store?}/{language?}/themes/global/assets/{asset}").To(c => c.GetGlobalAssets(null));

            routes.For<PageController>()
                .CreateRoute("{store?}/{language?}/pages/{page}").To(c => c.GetContentPageByName(null));

            routes.For<BlogController>()
                .CreateRoute("{store?}/{language?}/blogs/{blog}").To(c => c.GetBlog(null))
                .CreateRoute("{store?}/{language?}/blogs/{blog}/{article}").To(c => c.GetBlogArticle(null, null));

            #pragma warning restore 4014

            Func<string, Route> seoRouteFactory = url => new SeoRoute(url, new MvcRouteHandler(), workContextFactory, commerceCoreApi, staticContentService, cacheManager);
            routes.MapLocalizedStorefrontRoute(name: "SeoRoute", url: "{*path}", defaults: new { controller = "StorefrontHome", action = "Index" }, constraints: null, routeFactory: seoRouteFactory);
        }
    }
}