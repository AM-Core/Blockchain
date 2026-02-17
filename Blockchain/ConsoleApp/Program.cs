using Application.MiningApplication;
using Application.MiningApplication.Dispatching;
using ConsoleApp.Bootstrap;
using ConsoleApp.ConsoleHandler;
using Microsoft.Extensions.DependencyInjection;

var provider = DependencyBootstrapper.ConfigureServices();

var application = provider.GetRequiredService<ApplicationHandler>();
var consoleHandler = provider.GetRequiredService<ConsoleHandler>();

provider.GetRequiredService<LoadConfiguration>().LoadConfigs();
consoleHandler.Run(application);