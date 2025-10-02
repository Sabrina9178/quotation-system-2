using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuotationSystem2.PresentationLayer.Templates
{
    /// <summary>
    /// QuotationViewTemplate.xaml 的互動邏輯
    /// </summary>
    public partial class QuotationViewTemplate : UserControl
    {
        public QuotationViewTemplate()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(QuotationViewTemplate), new PropertyMetadata(""));
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty SheetProperty =
            DependencyProperty.Register(nameof(Sheet), typeof(object), typeof(QuotationViewTemplate), new PropertyMetadata(null));
        public object Sheet
        {
            get => GetValue(SheetProperty);
            set => SetValue(SheetProperty, value);
        }

        public static readonly DependencyProperty GridContentProperty =
            DependencyProperty.Register(nameof(GridContent), typeof(UIElement), typeof(QuotationViewTemplate), new PropertyMetadata(null));
        public UIElement GridContent
        {
            get => (UIElement)GetValue(GridContentProperty);
            set => SetValue(GridContentProperty, value);
        }

        public static readonly DependencyProperty SelectThisPageProperty =
            DependencyProperty.Register(nameof(SelectThisPage), typeof(ICommand), typeof(QuotationViewTemplate), new PropertyMetadata(null));
        public ICommand SelectThisPage
        {
            get => (ICommand)GetValue(SelectThisPageProperty);
            set => SetValue(SelectThisPageProperty, value);
        }

        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register(nameof(PageCount), typeof(string), typeof(QuotationViewTemplate), new PropertyMetadata(""));
        public string PageCount
        {
            get => (string)GetValue(PageCountProperty);
            set => SetValue(PageCountProperty, value);
        }

        #region Export Record Button
        public static readonly DependencyProperty ExportSelectedRecordProperty =
            DependencyProperty.Register(nameof(ExportSelectedRecord), typeof(ICommand), typeof(QuotationViewTemplate), new PropertyMetadata(null));
        public ICommand ExportSelectedRecord
        {
            get => (ICommand)GetValue(ExportSelectedRecordProperty);
            set => SetValue(ExportSelectedRecordProperty, value);
        }

        public static readonly DependencyProperty ExportAllRecordProperty =
            DependencyProperty.Register(nameof(ExportAllRecord), typeof(ICommand), typeof(QuotationViewTemplate), new PropertyMetadata(null));
        public ICommand ExportAllRecord
        {
            get => (ICommand)GetValue(ExportAllRecordProperty);
            set => SetValue(ExportAllRecordProperty, value);
        }
        #endregion

        #region Delete Record Button
        public static readonly DependencyProperty DeleteSelectedRecordProperty =
            DependencyProperty.Register(nameof(DeleteSelectedRecord), typeof(ICommand), typeof(QuotationViewTemplate), new PropertyMetadata(null));
        public ICommand DeleteSelectedRecord
        {
            get => (ICommand)GetValue(DeleteSelectedRecordProperty);
            set => SetValue(DeleteSelectedRecordProperty, value);
        }

        public static readonly DependencyProperty DeleteAllRecordProperty =
            DependencyProperty.Register(nameof(DeleteAllRecord), typeof(ICommand), typeof(QuotationViewTemplate), new PropertyMetadata(null));
        public ICommand DeleteAllRecord
        {
            get => (ICommand)GetValue(DeleteAllRecordProperty);
            set => SetValue(DeleteAllRecordProperty, value);
        }

        #endregion

        #region edit Record Button
        public static readonly DependencyProperty EditRecordProperty =
            DependencyProperty.Register(nameof(EditRecord), typeof(ICommand), typeof(QuotationViewTemplate), new PropertyMetadata(null));
        public ICommand EditRecord
        {
            get => (ICommand)GetValue(EditRecordProperty);
            set => SetValue(EditRecordProperty, value);
        }
        #endregion

        #region Page Navigation Buttons
        public static readonly DependencyProperty PreviousPageProperty =
            DependencyProperty.Register(nameof(PreviousPage), typeof(ICommand), typeof(QuotationViewTemplate), new PropertyMetadata(null));
        public ICommand PreviousPage
        {
            get => (ICommand)GetValue(PreviousPageProperty);
            set => SetValue(PreviousPageProperty, value);
        }

        public static readonly DependencyProperty NextPageProperty =
            DependencyProperty.Register(nameof(NextPage), typeof(ICommand), typeof(QuotationViewTemplate), new PropertyMetadata(null));
        public ICommand NextPage
        {
            get => (ICommand)GetValue(NextPageProperty);
            set => SetValue(NextPageProperty, value);
        }

        #endregion


        #region Visibility Properties
        public static readonly DependencyProperty EditRecordButtonVisibilityProperty =
            DependencyProperty.Register(nameof(EditRecordButtonVisibility), typeof(Visibility), typeof(QuotationViewTemplate), new PropertyMetadata(Visibility.Visible));
        public Visibility EditRecordButtonVisibility
        {
            get => (Visibility)GetValue(EditRecordButtonVisibilityProperty);
            set => SetValue(EditRecordButtonVisibilityProperty, value);
        }

        public static readonly DependencyProperty DeleteRecordButtonVisibilityProperty =
            DependencyProperty.Register(nameof(DeleteRecordButtonVisibility), typeof(Visibility), typeof(QuotationViewTemplate), new PropertyMetadata(Visibility.Visible));
        public Visibility DeleteRecordButtonVisibility
        {
            get => (Visibility)GetValue(DeleteRecordButtonVisibilityProperty);
            set => SetValue(DeleteRecordButtonVisibilityProperty, value);
        }

        public static readonly DependencyProperty ExportRecordButtonVisibilityProperty =
            DependencyProperty.Register(nameof(ExportRecordButtonVisibility), typeof(Visibility), typeof(QuotationViewTemplate), new PropertyMetadata(Visibility.Visible));
        public Visibility ExportRecordButtonVisibility
        {
            get => (Visibility)GetValue(ExportRecordButtonVisibilityProperty);
            set => SetValue(ExportRecordButtonVisibilityProperty, value);
        }

        public static readonly DependencyProperty SelectingCheckBoxVisibilityProperty =
            DependencyProperty.Register(nameof(SelectingCheckBoxVisibility), typeof(Visibility), typeof(QuotationViewTemplate), new PropertyMetadata(Visibility.Visible));
        public Visibility SelectingCheckBoxVisibility
        {
            get => (Visibility)GetValue(SelectingCheckBoxVisibilityProperty);
            set => SetValue(SelectingCheckBoxVisibilityProperty, value);
        }
        #endregion
    }
}
