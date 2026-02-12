using Application.MiningApplication;
using ConsoleApp.Bootstrap;
using ConsoleApp.ConsoleHandler;
using Microsoft.Extensions.DependencyInjection;

var provider = DependencyBootstrapper.ConfigureServices();
var application = provider.GetRequiredService<ApplicationHandler>();
var consoleHandler = new ConsoleHandler();
consoleHandler.Run(application);