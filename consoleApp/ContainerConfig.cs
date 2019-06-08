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
            builder.RegisterType<FileDownloader>().As<IFileDownloader>();
            builder.RegisterType<CardsProcessor>().As<ICardsProcessor>();
            builder.RegisterType<CardBuilder>().As<ICardBuilder>();
            builder.RegisterType<OxfordDictionariesRetriever>()
                .As<IDictionaryDataRetriever>()
                .SingleInstance();
            builder.RegisterType<ResourceManager>().As<IResourceManager>();
            builder.RegisterType<AnkiPackageDbContext>().As<IAnkiPackageDbContext>();
            builder.RegisterType<AnkiFieldsBuilder>().As<IAnkiFieldsBuilder>();
            builder.RegisterType<AnkiEntityBuilder>().As<IAnkiEntityBuilder>();
            builder.RegisterType<AnkiPackageBuilder>().As<IAnkiPackageBuilder>();

            return builder.Build();
        }
    }
}
