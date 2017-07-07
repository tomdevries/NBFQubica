using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NBF.Qubica.RestClient
{
    public static class XMLManager
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static string GetAPIVersion(string xmlString)
        {
            string version = null;
            try
            {
                #region xml example
                //<string>1.00.00</string>
                #endregion

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString); 

                XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    version = xmlNode["string"].InnerText;
                }
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Error parsing version from response: {0}", ex.Message));
            }

            return version;
        }

        public static S_Scores GetScores(string xmlString)
        {
            logger.Debug(xmlString);

            S_Scores scores = new S_Scores();
            scores.Events = new List<S_Event>();

            try
            {
                #region xml example
                //<Scores>
                //  <Events>
                //      <Event>....</Event>
                //      <Event>....</Event>
                //  </Events>
                //</Scores>
                #endregion

                if (xmlString != null)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xmlString);

                    XmlNodeList xmlEventNodeList = xmlDocument.SelectNodes("/Scores/Events/*");

                    // loop over all event nodes
                    foreach (XmlNode xmlEventNode in xmlEventNodeList)
                        scores.Events.Add(GetEvent(xmlEventNode));
                }
                else
                    logger.Warn("XML string is null");
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Error parsing scores from response: {0}", ex.Message));
            }

            return scores;
        }

        private static S_Event GetEvent(XmlNode xmlEventNode)
        {
            S_Event bowlEvent = new S_Event();
            bowlEvent.games = new List<S_Game>();

            try
            {
                #region xml example
                //<Event>
                //  <IDEvent>6</IDEvent>
                //  <OpenDateTime>201308270953</OpenDateTime>
                //  <CloseDateTime>201308271004</CloseDateTime>
                //  <Status>Played</Status>
                //  <OpenMode>Single</OpenMode>
                //  <Games>
                //      <Game>....</Game>
                //      <Game>....</Game>
                //  </Games>
                //</Event>
                #endregion

                // all child nodes in Event
                XmlNodeList xmlEventChildNodeList = xmlEventNode.SelectNodes("*");

                foreach (XmlNode xmlEventChildNode in xmlEventChildNodeList)
                {
                    switch (xmlEventChildNode.Name)
                    {
                        case "IDEvent":
                            bowlEvent.eventCode = Conversion.StringToInt(xmlEventChildNode.InnerText).Value;
                            break;
                        case "OpenDateTime":
                            bowlEvent.openDateTime = Conversion.StringToDateTimeQubica(xmlEventChildNode.InnerText).Value;
                            break;
                        case "CloseDateTime":
                            bowlEvent.closeDateTime = Conversion.StringToDateTimeQubica(xmlEventChildNode.InnerText).Value;
                            break;
                        case "Status":
                            bowlEvent.status = (Status)Enum.Parse(typeof(Status), xmlEventChildNode.InnerText);
                            break;
                        case "OpenMode":
                            bowlEvent.openMode = (OpenMode)Enum.Parse(typeof(OpenMode), xmlEventChildNode.InnerText);
                            break;
                        case "Games":
                            // all child nodes in Games (game)
                            XmlNodeList xmlGamesNodeList = xmlEventChildNode.SelectNodes("*");

                            // loop over all game nodes for this Event
                            foreach (XmlNode xmlGameNode in xmlGamesNodeList)
                                bowlEvent.games.Add(GetGame(xmlGameNode));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Error parsing scores from response: {0}", ex.Message));
            }

            return bowlEvent;
        }

        private static S_Game GetGame(XmlNode xmlGameNode)
        {
            S_Game game = new S_Game();
            game.frames = new List<S_Frame>();

            try
            {
                #region xml example
                //<Game>
                //  <IDGame>53</IDGame>
                //  <LaneNumber>1</LaneNumber>
                //  <PlayerName>Rita</PlayerName>
                //  <FullName>Rita</FullName>
                //  <FreeEntryCode/>
                //  <PlayerPosition>2</PlayerPosition>
                //  <StartDateTime>201308270953</StartDateTime>
                //  <EndDateTime>201308270955</EndDateTime>
                //  <GameNumber>1</GameNumber>
                //  <Hdcp>0</Hdcp>
                //  <Total>137</Total>
                //  <Frames>
                //      <Frame>....</Frame>
                //      <Frame>....</Frame>
                //  </Frames>
                //</Game>
                #endregion

                // all child nodes in Game
                XmlNodeList xmlGameChildNodeList = xmlGameNode.SelectNodes("*");

                foreach (XmlNode xmlGameChildNode in xmlGameChildNodeList)
                {
                    switch (xmlGameChildNode.Name)
                    {
                        case "IDGame":
                            game.gameCode = Conversion.StringToInt(xmlGameChildNode.InnerText).Value;
                            break;
                        case "LaneNumber":
                            game.laneNumber = Conversion.StringToInt(xmlGameChildNode.InnerText).Value;
                            break;
                        case "PlayerName":
                            game.playerName = Conversion.StringToString(xmlGameChildNode.InnerText);
                            break;
                        case "PlayerFullName":
                            game.fullName = Conversion.StringToString(xmlGameChildNode.InnerText);
                            break;
                        case "FreeEntryCode":
                            game.freeEntryCode = Conversion.StringToString(xmlGameChildNode.InnerText);
                            break;
                        case "PlayerPosition":
                            game.playerPosition = Conversion.StringToInt(xmlGameChildNode.InnerText).Value;
                            break;
                        case "StartDateTime":
                            game.startDateTime = Conversion.StringToDateTimeQubica(xmlGameChildNode.InnerText).Value;
                            break;
                        case "EndDateTime":
                            game.endDateTime = Conversion.StringToDateTimeQubica(xmlGameChildNode.InnerText).Value;
                            break;
                        case "GameNumber":
                            game.gameNumber = Conversion.StringToInt(xmlGameChildNode.InnerText).Value;
                            break;
                        case "Hdcp":
                            game.handicap = Conversion.StringToInt(xmlGameChildNode.InnerText).Value;
                            break;
                        case "Total":
                            game.total = Conversion.StringToInt(xmlGameChildNode.InnerText).Value;
                            break;
                        case "Frames":
                            // all child nodes in Games (game)
                            XmlNodeList xmlFramesNodeList = xmlGameChildNode.SelectNodes("*");

                            // loop over all Frame nodes for this Game
                            foreach (XmlNode xmlFrameNode in xmlFramesNodeList)
                                game.frames.Add(GetFrame(xmlFrameNode));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Error parsing Game from response: {0}", ex.Message));
            }

            return game;
        }

        private static S_Frame GetFrame(XmlNode xmlFrameNode)
        {
            S_Frame frame = new S_Frame();

            try
            {
                #region xml example
                //<Frame>
                //  <FrameNumber>1</FrameNumber>
                //  <ProgressiveTotal>8</ProgressiveTotal>
                //  <IsConvertedSplit>No</IsConvertedSplit>
                //  <Bowl1>..</Bowl1>
                //  <Bowl2>..</Bowl2>
                //  <Bowl3>..</Bowl3>
                //</Frame>
                #endregion

                // all child nodes in Frame
                XmlNodeList xmlFrameChildNodeList = xmlFrameNode.SelectNodes("*");

                foreach (XmlNode xmlFrameChildNode in xmlFrameChildNodeList)
                {
                    switch (xmlFrameChildNode.Name)
                    {
                        case "FrameNumber":
                            frame.frameNumber = Conversion.StringToInt(xmlFrameChildNode.InnerText).Value;
                            break;
                        case "ProgressiveTotal":
                            frame.progressiveTotal = Conversion.StringToInt(xmlFrameChildNode.InnerText).Value;
                            break;
                        case "IsConvertedSplit":
                            frame.isConvertedSplit = Conversion.StringToBoolQubica(xmlFrameChildNode.InnerText).Value;
                            break;
                        case "Bowl1":
                            frame.bowl1 = GetBowl(xmlFrameChildNode);
                            break;
                        case "Bowl2":
                            frame.bowl2 = GetBowl(xmlFrameChildNode);
                            break;
                        case "Bowl3":
                            frame.bowl3 = GetBowl(xmlFrameChildNode);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Error parsing Frame from response: {0}", ex.Message));
            }

            return frame;
        }

        private static S_Bowl GetBowl(XmlNode xmlBowlNode)
        {
            S_Bowl bowl = new S_Bowl();

            try
            {
                #region xml example
                //<Bowl>
                //  <Total>8</Total>
                //  <IsStrike>No</IsStrike>
                //  <IsSpare>No</IsSpare>
                //  <IsSplit>Yes</IsSplit>
                //  <IsGutter>No</IsGutter>
                //  <IsFoul>No</IsFoul>
                //  <IsManuallyModified>No</IsManuallyModified>
                //  <Pins>1,4,5,6,7,8,9,10</Pins>
                //</Bowl>
                #endregion

                // all child nodes in Event
                XmlNodeList xmlBowlChildNodeList = xmlBowlNode.SelectNodes("*");
                
                foreach (XmlNode xmlBowlChildNode in xmlBowlChildNodeList)
                {
                    switch (xmlBowlChildNode.Name)
                    {
                        case "Total": //totaal van de bowls in dit frame, dus niet de worp zelf
                            bowl.total = Conversion.StringToInt(xmlBowlChildNode.InnerText).Value;
                            break;
                        case "IsStrike":
                            bowl.isStrike = Conversion.StringToBoolQubica(xmlBowlChildNode.InnerText).Value;
                            break;
                        case "IsSpare":
                            bowl.isSpare = Conversion.StringToBoolQubica(xmlBowlChildNode.InnerText).Value;
                            break;
                        case "IsSplit":
                            bowl.isSplit = Conversion.StringToBoolQubica(xmlBowlChildNode.InnerText).Value;
                            break;
                        case "IsGutter":
                            bowl.isGutter = Conversion.StringToBoolQubica(xmlBowlChildNode.InnerText).Value;
                            break;
                        case "IsFoul":
                            bowl.isFoul = Conversion.StringToBoolQubica(xmlBowlChildNode.InnerText).Value;
                            break;
                        case "IsManuallyModified":
                            bowl.isManuallyModified = Conversion.StringToBoolQubica(xmlBowlChildNode.InnerText).Value;
                            break;
                        case "Pins":
                            bowl.pins = Conversion.StringToString(xmlBowlChildNode.InnerText);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Error parsing Bowl from response: {0}", ex.Message));
            }

            return bowl;
        }
    }
}
