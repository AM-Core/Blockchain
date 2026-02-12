using Application.MiningApplication;
using ConsoleApp.Bootstrap;
using ConsoleApp.ConsoleHandler;
using Domain;
using Microsoft.Extensions.DependencyInjection;

var provider = DependencyBootstrapper.ConfigureServices();
var application = provider.GetRequiredService<ApplicationHandler>();
var consoleHandler = new ConsoleHandler();
new LoadConfiguration().LoadConfigs();
consoleHandler.Run(application);