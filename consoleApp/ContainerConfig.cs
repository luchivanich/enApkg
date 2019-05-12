using ApkgCreator;
using Autofac;
using Cards;
using CardsBusinessLogic;
using OxfordDictionaries;

namespace consoleApp
{
    public static class ContainerConfig
    {
        public static IContainer Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<SettingsProvider>()
                .As<ICardsDbConnectionStringProvider>()
                .As<IOxfordCacheDbConnectionStringProvider>()
                .As<IOxfordDictionarySettingsProvider>()
                .SingleInstance();
            builder.RegisterType<OxfordDictionariesCacheDBContext>().As<IOxfordDictionariesCacheDBContext>().SingleInstance();
            builder.RegisterType<CardsProcessor>().As<ICardsProcessor>();
            builder.RegisterType<CardBuilder>().As<ICardBuilder>();
            builder.Register(ctx => new CardsDbContext(ctx.Resolve<ICardsDbConnectionStringProvider>())).As<ICardsDbContext>().SingleInstance();
            builder.RegisterType<OxfordDictionariesRetriever>()
                .As<IDefinitionRetriever>()
                .As<IExamplesRetriever>()
                .SingleInstance();
            builder.RegisterType<AnkiPackageDbContext>().As<IAnkiPackageDbContext>();
            builder.RegisterType<AnkiFieldsBuilder>().As<IAnkiFieldsBuilder>();
            builder.RegisterType<AnkiEntityBuilder>().As<IAnkiEntityBuilder>();
            builder.RegisterType<AnkiAdditionalModelsBuilder>().As<IAnkiAdditionalModelsBuilder>();
            builder.RegisterType<AnkiPackageBuilder>().As<IAnkiPackageBuilder>();

            return builder.Build();
        }
    }
}
