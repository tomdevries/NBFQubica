using NLog;
using System;
using System.Collections.Generic;
using NBF.Qubica.Classes;
using NBF.Qubica.Managers;
using NBF.Qubica.RestClient;
using System.Net;

namespace NBF.Qubica.PollService
{
    public class ServiceHandler
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public ServiceHandler()
        {
        }

        public static void RegisterServerCertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
        }

        /// <summary>
        /// </summary>
        public void Execute()
        {
            logger.Debug("RegisterServerCertificateValidation");
            RegisterServerCertificateValidation();

            // get all the bowling centers we want to poll
            List<S_BowlingCenter> bowlingCenters = BowlingCenterManager.GetBowlingCenters();
            logger.Info("texts count:" + bowlingCenters.Count);

            // foreach bolowing center poll the api and store all the data
            foreach (S_BowlingCenter bowlingCenter in bowlingCenters)
            {
                logger.Info(string.Format("Handling bowlingcenter: {0} {1} using uri : {2}", bowlingCenter.id, bowlingCenter.name, bowlingCenter.uri));
                string APIversion = XMLManager.GetAPIVersion(RestfullClient.GetAPIVersion(bowlingCenter.uri, bowlingCenter.centerId, bowlingCenter.appname, bowlingCenter.secretkey));
                logger.Info(APIversion);

                if (string.Compare(APIversion, bowlingCenter.APIversion) == 0)
                {
                    if (bowlingCenter.numberOfLanes != null)
                    {
                        if (bowlingCenter.numberOfLanes > 0)
                        {
                            DateTime now = DateTime.Now;
                            DateTime lastSyncDate =DateTime.MinValue;
                            if (bowlingCenter.lastSyncDate != null)
                                lastSyncDate = bowlingCenter.lastSyncDate.Value;

                            logger.Info("Start get data from server for bolwing center: " + bowlingCenter.id);
                            
                            // if previous days need to be synchronized, call GetScores, otherwise Call GetLastScores
                            // https://localhost:10011/Scores?lanes=1&fromDate=20141207&toDate=20141207

                            if (lastSyncDate.Year == now.Year && lastSyncDate.Month == now.Month && lastSyncDate.Day == now.Day)
                            {
                                string lanes = string.Concat("1-", bowlingCenter.numberOfLanes.ToString());
                                bowlingCenter.scores = XMLManager.GetScores(RestfullClient.GetLastScores(bowlingCenter.uri, bowlingCenter.centerId, lanes, bowlingCenter.appname, bowlingCenter.secretkey));
                            }
                            else
                            {
                                string lanes = "1";
                                for (int laneNr = 2; laneNr <= bowlingCenter.numberOfLanes; laneNr++)
                                    lanes = String.Concat(lanes, ",", laneNr.ToString());
   
                                bowlingCenter.scores = XMLManager.GetScores(RestfullClient.GetScores(bowlingCenter.uri, bowlingCenter.centerId, lanes, lastSyncDate, now, bowlingCenter.appname, bowlingCenter.secretkey));
                            }
                            logger.Info("Stop get data from server for bolwing center: " + bowlingCenter.id + ", start saving..");

                            bowlingCenter.lastSyncDate = DateTime.Now;

                            BowlingCenterManager.Save(bowlingCenter);
                            logger.Info("Done saveing for bolwing center: " + bowlingCenter.id);
                        }
                        else
                        {
                            logger.Warn(string.Format("Number of lanes is less then 0 for bowlingcneter '{0}'", bowlingCenter.name));
                        }
                    }
                    else
                    {
                        logger.Warn(string.Format("Number of lanes is empty for bowlingcneter '{0}'", bowlingCenter.name));
                    }
                }
                else
                {
                    logger.Warn(string.Format("APIversion '{0}' not supported", APIversion));
                }
            }
        }
    }
}

