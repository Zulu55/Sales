namespace Sales.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Resources;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string NoInternet
        {
            get { return Resource.NoInternet; }
        }

        public static string Products
        {
            get { return Resource.Products; }
        }

        public static string TurnOnInternet
        {
            get { return Resource.TurnOnInternet; }
        }

        public static string AddProduct
        {
            get { return Resource.AddProduct; }
        }

        public static string Description
        {
            get { return Resource.Description; }
        }

        public static string DescriptionPlaceholder
        {
            get { return Resource.DescriptionPlaceholder; }
        }

        public static string Price
        {
            get { return Resource.Price; }
        }

        public static string PricePlaceholder
        {
            get { return Resource.PricePlaceholder; }
        }

        public static string Remarks
        {
            get { return Resource.Remarks; }
        }

        public static string Save
        {
            get { return Resource.Save; }
        }

        public static string ChangeImage
        {
            get { return Resource.ChangeImage; }
        }

        public static string DescriptionError
        {
            get { return Resource.DescriptionError; }
        }

        public static string PriceError
        {
            get { return Resource.PriceError; }
        }

        public static string ImageSource
        {
            get { return Resource.ImageSource; }
        }

        public static string FromGallery
        {
            get { return Resource.FromGallery; }
        }

        public static string NewPicture
        {
            get { return Resource.NewPicture; }
        }

        public static string Cancel
        {
            get { return Resource.Cancel; }
        }

        public static string Delete
        {
            get { return Resource.Delete; }
        }

        public static string Edit
        {
            get { return Resource.Edit; }
        }

        public static string DeleteConfirmation
        {
            get { return Resource.DeleteConfirmation; }
        }

        public static string Yes
        {
            get { return Resource.Yes; }
        }

        public static string No
        {
            get { return Resource.No; }
        }

        public static string Confirm
        {
            get { return Resource.Confirm; }
        }
    }
}