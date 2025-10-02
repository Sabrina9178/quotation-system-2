using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuotationSystem2.ApplicationLayer.Interfaces;
using QuotationSystem2.ApplicationLayer.Services;
using QuotationSystem2.DataAccessLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Models;
using QuotationSystem2.DataAccessLayer.Models.Contexts;
using QuotationSystem2.DataAccessLayer.Models.QuoteModel2;
using QuotationSystem2.DataAccessLayer.Repositories;
using QuotationSystem2.PresentationLayer;
using QuotationSystem2.PresentationLayer.Services;
using QuotationSystem2.PresentationLayer.Services.Interfaces;
using QuotationSystem2.PresentationLayer.Services.Localization;
using QuotationSystem2.PresentationLayer.ViewModels;
using Serilog;
using Serilog.Core;
using System.Configuration;
using System.Windows;
using System.Windows.Threading;

namespace QuotationSystem2
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; } = null!;
        private ILogger<App>? _logger;

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .UseSerilog((context, services, configuration) =>
                {
                    configuration
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext() // Enrich logs with context information (class)
                        .WriteTo.Console(outputTemplate:
                            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                        .WriteTo.File("C:\\Users\\user\\Desktop\\log_.txt",
                            rollingInterval: RollingInterval.Day,
                            outputTemplate:
                            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}");
                })
                .ConfigureServices((context, services) =>
                {
                    // Data Access Layer
                    services.AddSingleton<ISystemRepo, SystemRepo>();
                    services.AddSingleton<IQuoteQueryRepo, QuoteQueryRepo>();
                    services.AddSingleton<IQuoteWriteRepo, QuoteWriteRepo>();
                    //services.AddSingleton<IRecordRepo, RecordRepo>();
                    services.AddDbContext<QuotationSystem2DBContext>(options =>
                        options.UseSqlServer("Server=DESKTOP-MOSMRPO;Database=QuotationSystem2DB;User ID=ssss9178;Password=00000000;TrustServerCertificate=True;Trusted_Connection=True"));
                    
                    // Application Layer
                    services.AddSingleton<ILanguageService>(LanguageService.Instance);
                    services.AddSingleton<IAppStateService, AppStateService>();
                    services.AddSingleton<IEmployeeService, EmployeeService>();
                    services.AddSingleton<IQuoteQueryService, QuoteQueryService>();
                    services.AddSingleton<IRecordService, RecordService>();
                    services.AddSingleton<IQuoteWriteService, QuoteWriteService>();


                    // Presentation Layer
                    services.AddSingleton<IErrorDialogService, ErrorDialogService>();
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddSingleton<MainWindow>();

                    // ViewModels
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<LoginVM>();
                    services.AddSingleton<HomeVM>();
                    services.AddSingleton<AccountVM>();
                    services.AddSingleton<PriceTableVM>();
                    services.AddSingleton<SealingPlateQuotationVM>();
                    services.AddSingleton<SheerStudQuotationVM>();
                    services.AddSingleton<WaterMeterQuotationVM>();

                    // 這裡註冊 ILogger<App>
                    services.AddLogging();

                    // 之後在 constructor 注入 ILogger<App>
                    services.AddSingleton<App>();
                })
                .Build();

            // 從 DI 取得 logger
            _logger = AppHost.Services.GetRequiredService<ILogger<App>>();

            // 訂閱全域未捕捉例外事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                AppHost.Start();

                Log.Information("Starting up Quotation System 2");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
                var errorDialogService = AppHost.Services.GetRequiredService<IErrorDialogService>();
                errorDialogService.ShowMessageAsync("ApplicationStartErrorMessage").GetAwaiter().GetResult();
                Environment.Exit(1);
            }

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            Log.Information("Exiting Quotation System 2");
            Log.CloseAndFlush();
            base.OnExit(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                _logger.LogError(ex, "Unhandled domain exception");
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            
            _logger.LogError(e.Exception, "Unhandled UI thread exception");
            e.Handled = true; // 可視需求決定是否中斷或忽略
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            _logger.LogError(e.Exception, "Unobserved task exception");
            e.SetObserved();
        }
    }

    //public partial class App : Application
    //{
    //    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    //    protected override void OnStartup(StartupEventArgs e)
    //    {


    //        var services = new ServiceCollection();

    //        // Data Access Layer
    //        services.AddSingleton<ISystemRepo, SystemRepo>();
    //        services.AddSingleton<SystemDbContext>();

    //        #region Service Registeration
    //        // Application Layer
    //        services.AddSingleton<ILanguageService>(LanguageService.Instance); // use a static instance
    //        services.AddSingleton<IAppStateService, AppStateService>();
    //        services.AddSingleton<IEmployeeService, EmployeeService>();

    //        // Presentation Layer
    //        services.AddSingleton<IErrorDialogService, ErrorDialogService>();
    //        services.AddSingleton<INavigationService, NavigationService>();

    //        // MainWindow
    //        services.AddSingleton<MainWindow>();

    //        #endregion

    //        #region ViewModel Registeration
    //        services.AddSingleton<MainViewModel>();
    //        services.AddSingleton<LoginVM>();
    //        services.AddSingleton<HomeVM>();
    //        services.AddSingleton<AccountVM>();
    //        services.AddSingleton<AccountManagementVM>();
    //        services.AddSingleton<PriceTableVM>();
    //        services.AddSingleton<SealingPlateQuotationVM>();
    //        services.AddSingleton<SheerStudQuotationVM>();
    //        services.AddSingleton<WaterMeterQuotationVM>();
    //        #endregion

    //        ServiceProvider = services.BuildServiceProvider();


    //    }
    //}

}
