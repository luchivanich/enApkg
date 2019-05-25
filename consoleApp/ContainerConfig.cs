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
                .As<IOxfordDictionarySettingsProvider>()
                .SingleInstance();
            builder.RegisterType<OxfordDictionariesCacheDbContext>().As<IOxfordDictionariesCacheDBContext>().SingleInstance();
            builder.RegisterType<CardsProcessor>().As<ICardsProcessor>();
            builder.RegisterType<CardBuilder>().As<ICardBuilder>();
            builder.RegisterType<CardsDbContext>().As<ICardsDbContext>();
            builder.RegisterType<OxfordDictionariesRetriever>()
                .As<IDefinitionRetriever>()
                .As<IExamplesRetriever>()
                .As<IAudioFileUrlRetriever>()
                .SingleInstance();
            builder.RegisterType<AnkiPackageDbContext>().As<IAnkiPackageDbContext>();
            builder.RegisterType<AnkiFieldsBuilder>().As<IAnkiFieldsBuilder>();
            builder.RegisterType<AnkiEntityBuilder>().As<IAnkiEntityBuilder>();
            builder.RegisterType<AnkiPackageBuilder>().As<IAnkiPackageBuilder>();

            return builder.Build();
        }
    }
}
