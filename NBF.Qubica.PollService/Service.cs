using NLog;
using System;
using System.Globalization;
using System.ServiceProcess;
using System.Timers;
using NBF.Qubica.Common;

namespace NBF.Qubica.PollService
{
    public partial class Service : ServiceBase
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        Timer timer = new Timer();
        bool busy = false;
        
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            try
            {
                logger.Info("Interval: " + Settings.Interval);

                timer.Interval = Settings.Interval;
                timer.Enabled = true;
                logger.Info("Service started.");
            }
            catch (FormatException ex)
            {
                logger.Error(string.Format("The service can not start since the inteval read from the settings is not correct: {0}", ex.Message));
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Unknown error starting the service: {0}", ex.Message));
            }
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            logger.Info("Service stopped.");
        }
        
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            logger.Info("Elapsed");

            if (!busy)
            {
                logger.Info("Not busy");

                busy = true;
                try
                {
                    logger.Info("Service Handler");
                    ServiceHandler serviceHandler = new ServiceHandler();

                    logger.Info("Execute");
                    serviceHandler.Execute();
                }
                catch (FormatException ex)
                {
                    logger.Error(string.Format("The Service can not read the configuration: {0}", ex.Message));
                }
                catch
                {
                }
                busy = false;
            }
        }
    }
}
