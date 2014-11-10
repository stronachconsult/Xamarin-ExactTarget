using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Content.Res;

namespace AmazonMapsSample
{
	public class CoffeeDetails
	{
		public static void Display (Context context, CoffeeShop shop)
		{
			PopupWindow popup = null;;

			var layout = LayoutInflater.FromContext (context).Inflate (Resource.Layout.shop_details, null);

			layout.FindViewById<TextView> (Resource.Id.shop_title).Text = shop.Title;
			layout.FindViewById<TextView> (Resource.Id.shop_address).Text = shop.Address;

			var phone = layout.FindViewById<TextView> (Resource.Id.shop_phone);
			var phoneSpan = new SpannableString (shop.Phone);
			phoneSpan.SetSpan (new UnderlineSpan (), 0, phoneSpan.Length(), (SpanTypes)0);
			phone.TextFormatted = phoneSpan;
			phone.Click += delegate {
				var callIntent = new Intent (Intent.ActionCall);
				callIntent.SetData(Android.Net.Uri.Parse ("tel:" + phone.Text));
				context.StartActivity (callIntent);
			};

			layout.FindViewById<Button>(Resource.Id.dismiss_button).Click += delegate {
				popup.Dismiss();
			};

			float widthPx = TypedValue.ApplyDimension (ComplexUnitType.Dip, 270, Resources.System.DisplayMetrics);
			float heightPx = TypedValue.ApplyDimension (ComplexUnitType.Dip, 350, Resources.System.DisplayMetrics);

			popup = new PopupWindow (layout, (int)widthPx, (int)heightPx, true);
			popup.ShowAtLocation (layout, GravityFlags.Center, 0, 0);
		}
	}
}

