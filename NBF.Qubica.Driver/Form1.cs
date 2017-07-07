using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using NBF.Qubica.Database;
using NBF.Qubica.Classes;
using NBF.Qubica.Managers;
using NBF.Qubica.Common;
using NLog;
using NBF.Qubica.PollService;
using System.ServiceModel.Web;

namespace NBF.Qubica.Driver
{
    public partial class frmMain : Form
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public frmMain()
        {
            InitializeComponent();

            FillBowlingCenter();

            this.tabControl.SelectedTab = tabControl.TabPages[1];
        }

        #region General
        private void btnConnectToDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Info("start connecting");
                DatabaseConnection database = new DatabaseConnection();
                database.OpenConnection();
                MessageBox.Show("Database Opened");
                database.CloseConnection();
                MessageBox.Show("Database Closed");
                logger.Info("end connecting");
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error : {0}", ex.Message));
            }
        }

        private void btnPollApi_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceHandler serviceHandler = new ServiceHandler();
                serviceHandler.Execute();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error : {0}", ex.Message));
            }
        }

        private void btnPurgeData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to purge all data from the database (this cannot be undone!!)?", "Purge Database", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                logger.Info("Start removing bowls");
                List<S_Bowl> bowls = BowlManager.GetBowls();
                foreach (S_Bowl bowl in bowls)
                    BowlManager.Delete(bowl.id);

                logger.Info("Start removing frames");
                List<S_Frame> frames = FrameManager.GetFrames();
                foreach (S_Frame frame in frames)
                    FrameManager.Delete(frame.id);

                logger.Info("Start removing games");
                List<S_Game> games = GameManager.GetGames();
                foreach (S_Game game in games)
                    GameManager.Delete(game.id);

                logger.Info("Start removing events");
                List<S_Event> events = EventManager.GetEvents();
                foreach (S_Event bowlEvent in events)
                    EventManager.Delete(bowlEvent.id);

                logger.Info("Start removing scores");
                List<S_Scores> scores = ScoresManager.GetScores();
                foreach (S_Scores score in scores)
                    ScoresManager.Delete(score.id);
            }
        }
        #endregion

        #region BowlingCenter
        private void FillBowlingCenter()
        {
            try
            {
                List<S_BowlingCenter> bowlingCenters = BowlingCenterManager.GetBowlingCenters();

                if (bowlingCenters != null && bowlingCenters.Count > 0)
                {
                    S_BowlingCenter bowlingCenter = bowlingCenters[0];
                    txtQBowlingCenterId.Text = bowlingCenter.id.ToString();
                    txtBowlingCenterName.Text = bowlingCenter.name.ToString();
                    txtBowlingCenterUri.Text = bowlingCenter.uri.ToString();
                    txtBowlingCenterPort.Text = bowlingCenter.centerId.ToString();
                    txtBowlingCenterAPIVersion.Text = bowlingCenter.APIversion.ToString();
                    txtBowlingCenterNumberOfLanes.Text = bowlingCenter.numberOfLanes.ToString();
                    txtBowlingCenterLastSyncDate.Text = bowlingCenter.lastSyncDate.ToString();
                }

                foreach (S_BowlingCenter bc in bowlingCenters)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = bc.name;
                    item.Value = bc.id;

                    comboBoxCenters.Items.Add(item);
                }
                comboBoxCenters.SelectedIndex = 0;

                dgvBowlingCenters.DataSource = bowlingCenters.ToArray();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }

        private void dgvBowlingCenters_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.Cells["Id"].Value != null)
            {
                txtQBowlingCenterId.Text = dataGridView.CurrentRow.Cells["Id"].Value.ToString();
                txtBowlingCenterName.Text = dataGridView.CurrentRow.Cells["Name"].Value.ToString();
                txtBowlingCenterPort.Text = Conversion.ValueToString(dataGridView.CurrentRow.Cells["CenterId"].Value);
                txtBowlingCenterNumberOfLanes.Text = Conversion.ValueToString(dataGridView.CurrentRow.Cells["NumberOfLanes"].Value);
                txtBowlingCenterAPIVersion.Text = Conversion.ValueToString(dataGridView.CurrentRow.Cells["APIVersion"].Value);
                txtBowlingCenterNumberOfLanes.Text = Conversion.ValueToString(dataGridView.CurrentRow.Cells["NumberOfLanes"].Value);
                txtBowlingCenterLastSyncDate.Text = Conversion.ValueToString(dataGridView.CurrentRow.Cells["LastSyncDate"].Value);
            }
        }

        private void dgvBowlingCenters_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.Cells["Id"].Value != null)
            {
                FillScores(Conversion.StringToInt(dataGridView.CurrentRow.Cells["Id"].Value.ToString()).Value);

                tclObjects.SelectedTab = tclObjects.TabPages["Scores"];
                tclObjects.SelectedIndex = 1;
            }
        }

        private void btnBowlingCenterInsert_Click(object sender, EventArgs e)
        {
            S_BowlingCenter bowlingCenter = new S_BowlingCenter();
            bowlingCenter.name = Conversion.StringToString(txtBowlingCenterName.Text);
            bowlingCenter.uri = Conversion.StringToString(txtBowlingCenterUri.Text);
            bowlingCenter.centerId = Conversion.StringToInt(txtBowlingCenterPort.Text);
            bowlingCenter.numberOfLanes = Conversion.StringToInt(txtBowlingCenterNumberOfLanes.Text);
            bowlingCenter.lastSyncDate = Conversion.StringToDate(txtBowlingCenterLastSyncDate.Text);
            bowlingCenter.APIversion = Conversion.StringToString(txtBowlingCenterAPIVersion.Text);

            if (!BowlingCenterManager.BowlingCenterExistByName(bowlingCenter.name))
            {
                BowlingCenterManager.Insert(bowlingCenter);
                FillBowlingCenter();
            }
            else
                MessageBox.Show("BowlingCenter exists by name");
        }

        private void btnBowlingCenterUpdate_Click(object sender, EventArgs e)
        {
            S_BowlingCenter bowlingCenter = BowlingCenterManager.GetBowlingCenterById(Conversion.StringToInt(txtQBowlingCenterId.Text).Value);

            S_BowlingCenter bowlingCenterToUpdate = new S_BowlingCenter();
            bowlingCenterToUpdate.id = Conversion.StringToInt(txtQBowlingCenterId.Text).Value;
            bowlingCenterToUpdate.name = Conversion.StringToString(txtBowlingCenterName.Text);
            bowlingCenterToUpdate.uri = Conversion.StringToString(txtBowlingCenterUri.Text);
            bowlingCenterToUpdate.centerId = Conversion.StringToInt(txtBowlingCenterPort.Text);
            bowlingCenterToUpdate.numberOfLanes = Conversion.StringToInt(txtBowlingCenterNumberOfLanes.Text);
            bowlingCenterToUpdate.lastSyncDate = Conversion.StringToDate(txtBowlingCenterLastSyncDate.Text);
            bowlingCenterToUpdate.APIversion = Conversion.StringToString(txtBowlingCenterAPIVersion.Text);

            if (String.Compare(bowlingCenter.name, bowlingCenterToUpdate.name) == 0)
                BowlingCenterManager.Update(bowlingCenterToUpdate);
            else
                if (BowlingCenterManager.BowlingCenterExistByName(bowlingCenterToUpdate.name))
                    MessageBox.Show("BowlingCenter exists by name");
                else
                    BowlingCenterManager.Update(bowlingCenterToUpdate);

            FillBowlingCenter();
        }

        private void btnBowlingCenterDelete_Click(object sender, EventArgs e)
        {
            if (BowlingCenterManager.BowlingCenterExistById(Conversion.StringToInt(txtQBowlingCenterId.Text).Value))
            {
                if (!ScoresManager.ScoresExistByBowlingCenterId(Conversion.StringToInt(txtQBowlingCenterId.Text).Value))
                {
                    if (MessageBox.Show(String.Format("Are you sure to delete {0}", txtBowlingCenterName.Text), "Delete?", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        BowlingCenterManager.Delete(Conversion.StringToInt(txtQBowlingCenterId.Text).Value);
                        FillBowlingCenter();
                    }
                }
                else
                    MessageBox.Show("Scores exists, cannot delete");
            }
            else
                MessageBox.Show("BowlingCenter does not exists by name");
        }
        #endregion

        #region Scores
        private void FillScores(long bowlingCenterId)
        {
            S_Scores score = ScoresManager.GetScoresByBowlingCenterId(bowlingCenterId);

            if (score != null)
            {
                txtScoresId.Text = score.id.ToString();
                txtScoresBowlingCenterId.Text = score.bowlingCenterId.ToString();
            }

            List<S_Scores> scores = new List<S_Scores>();
            scores.Add(score);
            dgvScores.DataSource = scores.ToArray();
        }

        private void dgvScores_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.Cells["Id"].Value != null)
            {
                txtScoresId.Text = dataGridView.CurrentRow.Cells["Id"].Value.ToString();
                txtScoresBowlingCenterId.Text = dataGridView.CurrentRow.Cells["BowlingCenterId"].Value.ToString();
            }
        }

        private void dgvScores_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.Cells["Id"].Value != null)
            {
                FillEvents(Conversion.StringToInt(dataGridView.CurrentRow.Cells["Id"].Value.ToString()).Value);

                tclObjects.SelectedTab = tclObjects.TabPages["Events"];
                tclObjects.SelectedIndex = 2;
            }
        }

        private void btnScoresInsert_Click(object sender, EventArgs e)
        {
            S_Scores scores = new S_Scores();
            scores.bowlingCenterId = Conversion.StringToInt(txtScoresBowlingCenterId.Text).Value;

            if (!ScoresManager.ScoresExistByBowlingCenterId(scores.bowlingCenterId))
            {
                ScoresManager.Insert(scores);
                FillScores(scores.bowlingCenterId);
            }
            else
                MessageBox.Show("Score exists for bowlingcenter");
        }

        private void btnScoresUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Update not allowed");
        }

        private void btnScoresDelete_Click(object sender, EventArgs e)
        {
            if (ScoresManager.ScoresExistById(Conversion.StringToInt(txtScoresId.Text).Value))
            {
                if (!EventManager.EventExistByScoreId(Conversion.StringToInt(txtScoresId.Text).Value))
                {
                    if (MessageBox.Show(String.Format("Are you sure to delete {0}", txtScoresId.Text), "Delete?", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        ScoresManager.Delete(Conversion.StringToInt(txtScoresId.Text).Value);
                        tclObjects.SelectedTab = tclObjects.TabPages["BowlingCenter"];
                        tclObjects.SelectedIndex = 0;
                    }
                }
                else
                    MessageBox.Show("Event exists, cannot delete");
            }
            else
                MessageBox.Show("Scores does not exists by scoresid");
        }
        #endregion

        #region Events
        private void FillEvents(long scoresId)
        {
            List<S_Event> events = EventManager.GetEventsByScoreId(scoresId);

            if (events.Count > 0)
            {
                S_Event bowlEvent = events[0];
                txtEventsId.Text = bowlEvent.id.ToString();
                txtEventsScoresId.Text = bowlEvent.scoresId.ToString();
                txtEventsOpenDateTime.Text = bowlEvent.openDateTime.ToString();
                txtEventsCloseDateTime.Text = bowlEvent.closeDateTime.ToString();
                comStatus.Text = bowlEvent.status.ToString();
                comOpenMode.Text = bowlEvent.openMode.ToString();
            }

            dgvEvents.DataSource = events.ToArray();
        }

        private void dvgEvents_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.Cells["Id"].Value != null)
            {
                txtEventsId.Text = dataGridView.CurrentRow.Cells["Id"].Value.ToString();
                txtEventsScoresId.Text = dataGridView.CurrentRow.Cells["ScoresId"].Value.ToString();
                txtEventsOpenDateTime.Text = dataGridView.CurrentRow.Cells["OpenDateTime"].Value.ToString();
                txtEventsCloseDateTime.Text = dataGridView.CurrentRow.Cells["CloseDateTime"].ToString();
                comStatus.Text = dataGridView.CurrentRow.Cells["Status"].Value.ToString();
                comOpenMode.Text = dataGridView.CurrentRow.Cells["OpenMode"].Value.ToString();
            }
        }

        private void dvgEvents_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.Cells["Id"].Value != null)
            {
                FillGames(Conversion.StringToInt(dataGridView.CurrentRow.Cells["Id"].Value.ToString()).Value);

                tclObjects.SelectedTab = tclObjects.TabPages["Games"];
                tclObjects.SelectedIndex = 3;
            }
        }

        private void btnEventsInsert_Click(object sender, EventArgs e)
        {
            S_Event bowlEvent = new S_Event();
            bowlEvent.scoresId = Conversion.StringToInt(txtEventsScoresId.Text).Value;
            bowlEvent.openDateTime = Conversion.StringToDate(txtEventsOpenDateTime.Text).Value;
            bowlEvent.closeDateTime = Conversion.StringToDate(txtEventsCloseDateTime.Text);
            bowlEvent.status = (Status)Enum.Parse(typeof(Status), Conversion.StringToString(comStatus.Text));
            bowlEvent.openMode = (OpenMode)Enum.Parse(typeof(OpenMode), Conversion.StringToString(comOpenMode.Text));

            if (!EventManager.EventExistByScoreIdAndEventCode(bowlEvent.scoresId, bowlEvent.eventCode))
            {
                EventManager.Insert(bowlEvent);
                FillEvents(bowlEvent.scoresId);
            }
            else
                MessageBox.Show("Event exists by scores and opendatetime");
        }

        private void btnEventsUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Update not allowed");
        }

        private void btnEventsDelete_Click(object sender, EventArgs e)
        {
            if (EventManager.EventExistById(Conversion.StringToInt(txtEventsId.Text).Value))
            {
                if (!GameManager.GameExistByEventId(Conversion.StringToInt(txtEventsId.Text).Value))
                {
                    if (MessageBox.Show(String.Format("Are you sure to delete {0}", txtEventsId.Text), "Delete?", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        EventManager.Delete(Conversion.StringToInt(txtEventsId.Text).Value);
                        FillEvents(Conversion.StringToInt(txtEventsScoresId.Text).Value);
                    }
                }
                else
                    MessageBox.Show("Games exists, cannot delete");
            }
            else
                MessageBox.Show("Event does not exists by eventid");
        }
        #endregion

        #region Games
        private void FillGames(long eventId)
        {
            List<S_Game> games = GameManager.GetGamesByEventId(eventId);

            if (games.Count > 0)
            {
                S_Game game = games[0];
                txtGamesId.Text = game.id.ToString();
                txtGamesEventId.Text = game.eventId.ToString();
                txtGamesLaneNumber.Text = game.laneNumber.ToString();
                txtGamesPlayerName.Text = game.playerName.ToString();
                txtGamesFullName.Text = game.fullName.ToString();
                txtGamesFreeEntryCode.Text = game.freeEntryCode.ToString();
                txtGamesPlayerPosition.Text = game.playerPosition.ToString();
                txtGamesStartDateTime.Text = game.startDateTime.ToString();
                txtGamesEndDateTime.Text = game.endDateTime.ToString();
                txtGamesGameNumber.Text = game.gameNumber.ToString();
                txtGamesHandicap.Text = game.handicap.ToString();
                txtGamesTotal.Text = game.total.ToString();
            }

            dgvGames.DataSource = games.ToArray();
        }

        private void dgvGames_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.Cells["Id"].Value != null)
            {
                txtGamesId.Text = dataGridView.CurrentRow.Cells["Id"].Value.ToString();
                txtGamesEventId.Text = dataGridView.CurrentRow.Cells["EventId"].Value.ToString();
                txtGamesLaneNumber.Text = dataGridView.CurrentRow.Cells["LaneNumber"].Value.ToString();
                txtGamesPlayerName.Text = dataGridView.CurrentRow.Cells["PlayerName"].Value.ToString();
                txtGamesFullName.Text = dataGridView.CurrentRow.Cells["FullName"].Value.ToString();
                txtGamesFreeEntryCode.Text = dataGridView.CurrentRow.Cells["FreeEntryCode"].Value.ToString();
                txtGamesStartDateTime.Text = dataGridView.CurrentRow.Cells["StartDateTime"].Value.ToString();
                txtGamesEndDateTime.Text = dataGridView.CurrentRow.Cells["EndDateTime"].ToString();
                txtGamesGameNumber.Text = dataGridView.CurrentRow.Cells["GameNumber"].Value.ToString();
                txtGamesHandicap.Text = dataGridView.CurrentRow.Cells["Handicap"].ToString();
                txtGamesTotal.Text = dataGridView.CurrentRow.Cells["Total"].ToString();
            }
        }

        private void dgvGames_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.Cells["Id"].Value != null)
            {
                FillFrames(Conversion.StringToInt(dataGridView.CurrentRow.Cells["Id"].Value.ToString()).Value);

                tclObjects.SelectedTab = tclObjects.TabPages["Frames"];
                tclObjects.SelectedIndex = 4;
            }
        }

        private void btnGamesInsert_Click(object sender, EventArgs e)
        {
            S_Game game = new S_Game();
            game.eventId = Conversion.StringToInt(txtGamesEventId.Text).Value;
            game.laneNumber = Conversion.StringToInt(txtGamesLaneNumber.Text).Value;
            game.playerName = Conversion.StringToString(txtGamesPlayerName.Text);
            game.fullName = Conversion.StringToString(txtGamesFullName.Text);
            game.freeEntryCode = Conversion.StringToString(txtGamesFreeEntryCode.Text);
            game.playerPosition = Conversion.StringToInt(txtGamesPlayerPosition.Text).Value;
            game.startDateTime = Conversion.StringToDate(txtGamesStartDateTime.Text).Value;
            game.endDateTime = Conversion.StringToDate(txtGamesEndDateTime.Text);
            game.gameNumber = Conversion.StringToInt(txtGamesGameNumber.Text).Value;
            game.handicap = Conversion.StringToInt(txtGamesHandicap.Text).Value;
            game.total = Conversion.StringToInt(txtGamesTotal.Text).Value;

            if (!GameManager.GameExistByEventIdAndGameCode(game.eventId, game.gameCode))
            {
                GameManager.Insert(game);
                FillGames(game.eventId);
            }
            else
                MessageBox.Show("Game exists by eventid and gamecode");
        }

        private void btnGamesUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Update not allowed");
        }

        private void btnGamesDelete_Click(object sender, EventArgs e)
        {
            if (GameManager.GameExistById(Conversion.StringToInt(txtGamesId.Text).Value))
            {
                if (!FrameManager.FrameExistByGameId(Conversion.StringToInt(txtGamesId.Text).Value))
                {
                    if (MessageBox.Show(String.Format("Are you sure to delete {0}", txtGamesId.Text), "Delete?", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        GameManager.Delete(Conversion.StringToInt(txtGamesId.Text).Value);
                        FillGames(Conversion.StringToInt(txtGamesEventId.Text).Value);
                    }
                }
                else
                    MessageBox.Show("Frames exists, cannot delete");
            }
            else
                MessageBox.Show("Game does not exists by gameid");
        }
        #endregion

        #region Frames
        private void FillFrames(long gameId)
        {
            List<S_Frame> frames = FrameManager.GetFramesByGameid(gameId);

            if (frames.Count > 0)
            {
                S_Frame frame = frames[0];
                txtFramesId.Text = frame.id.ToString();
                txtFramesGameId.Text = frame.gameId.ToString();
                txtFramesFrameNumber.Text = frame.frameNumber.ToString();
                txtFramesProgressiveTotal.Text = frame.progressiveTotal.ToString();
                cbxFramesIsConvertedSplit.Checked = frame.isConvertedSplit;
            }

            dgvFrames.DataSource = frames.ToArray();
        }

        private void dgvFrames_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.Cells["Id"].Value != null)
            {
                txtFramesId.Text = dataGridView.CurrentRow.Cells["Id"].Value.ToString();
                txtFramesGameId.Text = dataGridView.CurrentRow.Cells["GameId"].Value.ToString();
                txtFramesFrameNumber.Text = dataGridView.CurrentRow.Cells["FrameNumber"].Value.ToString();
                txtFramesProgressiveTotal.Text = dataGridView.CurrentRow.Cells["ProgressiveTotal"].ToString();
                cbxFramesIsConvertedSplit.Checked = Conversion.StringToBool(dataGridView.CurrentRow.Cells["IsConvertedSplit"].ToString()).Value;
            }
        }

        private void dgvFrames_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.Cells["Id"].Value != null)
            {
                FillBowls(Conversion.StringToInt(dataGridView.CurrentRow.Cells["Id"].Value.ToString()).Value);

                tclObjects.SelectedTab = tclObjects.TabPages["Bowls"];
                tclObjects.SelectedIndex = 5;
            }
        }

        private void btnFramesInsert_Click(object sender, EventArgs e)
        {
            S_Frame frame = new S_Frame();
            frame.gameId = Conversion.StringToInt(txtFramesGameId.Text).Value;
            frame.frameNumber = Conversion.StringToInt(txtFramesFrameNumber.Text).Value;
            frame.progressiveTotal = Conversion.StringToInt(txtFramesProgressiveTotal.Text);
            frame.isConvertedSplit = cbxFramesIsConvertedSplit.Checked;

            if (!FrameManager.FrameExistByGameIdAndFrameNumber(frame.gameId, frame.frameNumber))
            {
                FrameManager.Insert(frame);
                FillFrames(frame.gameId);
            }
            else
                MessageBox.Show("Frame exists by gameid and framenumber");
        }

        private void btnFramesUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Update not allowed");
        }

        private void btnFramesDelete_Click(object sender, EventArgs e)
        {
            if (FrameManager.FrameExistById(Conversion.StringToInt(txtFramesId.Text).Value))
            {
                if (!BowlManager.BowlExistByFrameId(Conversion.StringToInt(txtFramesId.Text).Value))
                {
                    if (MessageBox.Show(String.Format("Are you sure to delete {0}", txtFramesId.Text), "Delete?", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        FrameManager.Delete(Conversion.StringToInt(txtFramesId.Text).Value);
                        FillFrames(Conversion.StringToInt(txtFramesGameId.Text).Value);
                    }
                }
                else
                    MessageBox.Show("Bowl exists, cannot delete");
            }
            else
                MessageBox.Show("Frame does not exists by frameid");
        }
        #endregion

        #region Bowls
        private void FillBowls(long frameId)
        {
            List<S_Bowl> bowls = BowlManager.GetBowlsByFrameId(frameId);

            if (bowls.Count > 0)
            {
                S_Bowl bowl = bowls[0];
                txtBowlsId.Text = bowl.id.ToString();
                txtBowlsFrameId.Text = bowl.frameId.ToString();
                txtBowlsBowlNumber.Text = bowl.bowlNumber.ToString();
                txtBowlsTotal.Text = bowl.total.ToString();
                cbxBowlsIsStrike.Checked = bowl.isStrike;
                cbxBowlsIsSpare.Checked = bowl.isSpare;
                cbxBowlsIsSplit.Checked = bowl.isSplit;
                cbxBowlsIsGutter.Checked = bowl.isGutter;
                cbxBowlsIsFoul.Checked = bowl.isFoul;
                cbxBowlsIsManuallyModified.Checked = bowl.isManuallyModified;
                txtBowlsPins.Text = bowl.pins.ToString();
            }

            dgvBowls.DataSource = bowls.ToArray();
        }

        private void dgvBowls_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.Cells["Id"].Value != null)
            {
                txtBowlsId.Text = dataGridView.CurrentRow.Cells["Id"].Value.ToString();
                txtBowlsFrameId.Text = dataGridView.CurrentRow.Cells["FrameId"].Value.ToString();
                txtBowlsBowlNumber.Text = dataGridView.CurrentRow.Cells["BowlNumber"].Value.ToString();
                txtBowlsTotal.Text = dataGridView.CurrentRow.Cells["Total"].Value.ToString();
                cbxBowlsIsStrike.Checked = Conversion.StringToBool(dataGridView.CurrentRow.Cells["IsStrike"].Value.ToString()).Value;
                cbxBowlsIsSpare.Checked = Conversion.StringToBool(dataGridView.CurrentRow.Cells["IsSpare"].Value.ToString()).Value;
                cbxBowlsIsSplit.Checked = Conversion.StringToBool(dataGridView.CurrentRow.Cells["IsSplit"].Value.ToString()).Value;
                cbxBowlsIsGutter.Checked = Conversion.StringToBool(dataGridView.CurrentRow.Cells["IsGutter"].Value.ToString()).Value;
                cbxBowlsIsFoul.Checked = Conversion.StringToBool(dataGridView.CurrentRow.Cells["IsFoul"].Value.ToString()).Value;
                cbxBowlsIsManuallyModified.Checked = Conversion.StringToBool(dataGridView.CurrentRow.Cells["IsManuallyModified"].Value.ToString()).Value;
                txtBowlsPins.Text = dataGridView.CurrentRow.Cells["Pins"].Value.ToString();
            }
        }

        private void btnBowlsInsert_Click(object sender, EventArgs e)
        {
            S_Bowl bowl = new S_Bowl();
            bowl.frameId = Conversion.StringToInt(txtBowlsFrameId.Text).Value;
            bowl.bowlNumber = Conversion.StringToInt(txtBowlsBowlNumber.Text).Value;
            bowl.total = Conversion.StringToInt(txtBowlsTotal.Text);
            bowl.isStrike = cbxBowlsIsStrike.Checked;
            bowl.isSpare = cbxBowlsIsSpare.Checked;
            bowl.isSplit = cbxBowlsIsSplit.Checked;
            bowl.isGutter = cbxBowlsIsGutter.Checked;
            bowl.isFoul = cbxBowlsIsFoul.Checked;
            bowl.isManuallyModified = cbxBowlsIsManuallyModified.Checked;
            bowl.pins = Conversion.StringToString(txtBowlsPins.Text);

            if (!BowlManager.BowlExistByFrameIdAndBowlNumber(bowl.frameId, bowl.bowlNumber))
            {
                BowlManager.Insert(bowl);
                FillBowls(bowl.frameId);
            }
            else
                MessageBox.Show("Bowl exists by frameid and bowlnumber");
        }

        private void btnBowlsUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Update not allowed");
        }

        private void btnBowlsDelete_Click(object sender, EventArgs e)
        {
            if (BowlManager.BowlExistById(Conversion.StringToInt(txtBowlsId.Text).Value))
            {
                if (MessageBox.Show(String.Format("Are you sure to delete {0}", txtBowlsId.Text), "Delete?", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BowlManager.Delete(Conversion.StringToInt(txtBowlsId.Text).Value);
                    FillBowls(Conversion.StringToInt(txtBowlsFrameId.Text).Value);
                }
            }
            else
                MessageBox.Show("Bowl does not exists by bowlid");
        }
        #endregion

        #region rest
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                string requestData = "{\"username\":\"" + txtUsername.Text + "\",\"password\":\"" + txtPassword.Text + "\",\"frequentbowlernumber\":\"" + txtFrequentBowlerNumber.Text + "\"}";
                byte[] data = Encoding.UTF8.GetBytes(requestData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/auth/login");
                request.Method = "POST";
                request.ContentType = "application/json";
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/auth/logout/"+txtLogoutUserID.Text);
                request.Method = "DELETE";
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/" + txtProfileUserID.Text + "/profile");
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (WebFaultException<ErrorData> wfe)
            {
                ResponseTextBox.Text = wfe.Action + " " + wfe.Code + " " + wfe.Detail.Reason + " " + wfe.Detail.DetailedInformation;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnOtherProfile_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/" + txtProfileUserID.Text + "/profile/" + txtOtherProfileUserID.Text);
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnGames_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/" + txtGamesUserID.Text + "/games");
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnOtherGames_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/" + txtGamesUserID.Text + "/games/" + txtOtherGamesUserID.Text);
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }
        
        private void btnGamesPlayed_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/" + txtGamesPlayedUserID.Text + "/games/bowling/" + txtGamesPlayedBowlingCenterId.Text+ "/" + txtGamesPlayedPlaydate.Text +"/1");
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnOtherGamesPlayed_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/" + txtGamesPlayedUserID.Text + "/games/bowling/" + txtGamesPlayedBowlingCenterId.Text + "/" + txtGamesPlayedPlaydate.Text + "/" + txtOtherGamesPlayedUserID.Text);
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnGame_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/" + txtGameUserId.Text + "/games/game/" + txtGameId.Text);
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnFavorits_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/" + txtFavoritsUserID.Text + "/profile/favorites");
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnFavoritsAdd_Click(object sender, EventArgs e)
        {

            try
            {
                ResponseTextBox.Text = "";

                string requestData = "{\"userid\":" + txtFavoritsUserID.Text + ", \"favorituserid\":" + txtFavoritsAddDeleteUserID.Text + "}";
                byte[] data = Encoding.UTF8.GetBytes(requestData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/profile/favorites");
                request.Method = "POST";
                request.ContentType = "application/json";
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnFavoritDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/users/" + txtFavoritsUserID.Text + "/profile/favorites/" + txtFavoritsAddDeleteUserID.Text);
                request.Method = "DELETE";
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnBowlingCenters_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/bowlingcenters/" + txtBowlingCenterUserId.Text);
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnBowlingCenter_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/bowlingcenters/" + txtBCUserId.Text + "/" + txtBCBCid.Text);
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnBowlingcenterLanes_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/bowlingcenters/" + txtBCUserId.Text + "/" + txtBCBCid.Text + "/Lanes");
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnLane_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/bowlingcenters/" + txtBCUserId.Text + "/" +txtBCBCid.Text + "/Lanes/" + txtLaneId.Text);
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/search/" + txtSearchUserId.Text + "/" + txtSearch.Text);
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnNbf_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/nbf/" + txtNBFUserId.Text);
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        private void btnGetAdvert_Click(object sender, EventArgs e)
        {
            try
            {
                ResponseTextBox.Text = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtBaseURL.Text + "/v1/advertisement/" + txtNBFUserId.Text);
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseTextBox.Text = result;
            }
            catch (Exception ex)
            {
                ResponseTextBox.Text = ex.Message;
            }
        }

        #endregion

        private void createDummyData_Click(object sender, EventArgs e)
        {
            List<S_BowlingCenter> lbc = BowlingCenterManager.GetBowlingCenters();
            foreach (S_BowlingCenter bc in lbc)
            {
                logger.Info(bc.name);
                for (int sc_i=1; sc_i <= 1; sc_i++)
                {
                    logger.Debug("score: " + sc_i);
                    long? scID= null;

                    if (!ScoresManager.ScoresExistByBowlingCenterId((long)bc.id))
                    {
                        S_Scores sc = new S_Scores();
                        sc.bowlingCenterId = (long)bc.id;
                        scID = ScoresManager.Insert(sc);
                    }
                    else
                    {
                        scID = ScoresManager.GetScoresByBowlingCenterId((long)bc.id).id;
                    }

                    for (int e_i = 1; e_i <= 7; e_i++)
                    {
                        logger.Debug("event: " + e_i + " of seven");
                        S_Event bowlevent = new S_Event();
                        Random r = new Random();
                        int number_of_days = r.Next(10,90) * -1;
                        bowlevent.closeDateTime = DateTime.Now.AddDays(number_of_days);
                        bowlevent.eventCode = e_i;
                        bowlevent.openDateTime = (DateTime)bowlevent.closeDateTime;
                        bowlevent.openMode = OpenMode.Single;
                        bowlevent.scoresId = (long)scID;
                        bowlevent.status = Status.Played;

                        long? eID = EventManager.Insert(bowlevent);

                        string[] fec = new string[10] { "10-575757", "11-575757", "12-575757", "13-575757", "14-575757", "15-575757", "16-575757", "17-575757", "18-575757", "19-575757" };
                        string[] fn = new string[10] { "Tom de Vries", "Rick de Vries", "Arjan de Vries", "Stefan de Vries", "Robin de Vries", "Brent de Vries", "Sebastiaan de Vries", "Dominique de Vries", "Martijn de Vries", "Wouter de Vries" };
                        string[] pn = new string[10] { "Tom", "Rick", "Arjan", "Stefan", "Robin", "Brent", "Sebastiaan", "Dominique", "Martijn", "Wouter" };

                        
                        for (int g_i = 1; g_i <= 10; g_i++)
                        {
                            logger.Debug("game: " + g_i + " of ten");
                            S_Game game = new S_Game();
                            game.endDateTime = bowlevent.openDateTime;
                            game.eventId = (long)eID;
                            game.freeEntryCode = fec[g_i - 1];
                            game.fullName = fn[g_i - 1];
                            game.gameCode = g_i;
                            game.gameNumber = g_i;
                            game.handicap = 0;
                            game.laneNumber = r.Next(1,4);
                            game.playerName = pn[g_i - 1];
                            game.playerPosition = 1;
                            game.startDateTime = bowlevent.openDateTime;
                            game.total = r.Next(80,210);
                            game.totalSparesInGame = r.Next(3,8);
                            game.totalSparesInMonth = r.Next(20,45);
                            game.totalStrikesInGame = r.Next(1, 5);
                            game.totalStrikesInMonth = r.Next(10, 15);

                            long? gID = GameManager.Insert(game);

                            for (int f_i = 1; f_i < 10; f_i++)
                            {
                                logger.Debug("frame: " + f_i + " of ten");
                                S_Frame f = new S_Frame();
                                f.frameNumber = f_i;
                                f.gameId = (long)gID;
                                f.isConvertedSplit = false;
                                f.progressiveTotal = 0;

                                long? fID = FrameManager.Insert(f);

                                S_Bowl b1 = new S_Bowl();
                                b1.bowlNumber = 1;
                                b1.frameId = (long)fID;
                                b1.isFoul = false;
                                b1.isGutter = false;
                                b1.isManuallyModified = false;
                                b1.isSpare = true;
                                b1.isSplit = false;
                                b1.isStrike = false;
                                b1.pins = "1,2,3,4,5,6,7";
                                b1.total = 10;

                                S_Bowl b2 = new S_Bowl();
                                b2.bowlNumber = 2;
                                b2.frameId = (long)fID;
                                b2.isFoul = false;
                                b2.isGutter = false;
                                b2.isManuallyModified = false;
                                b2.isSpare = true;
                                b2.isSplit = false;
                                b2.isStrike = false;
                                b2.pins = "8,9,10";
                                b2.total = 10;

                                BowlManager.Insert(b1);
                                BowlManager.Insert(b2);
                            }
                        }
                    }
                }
            }
        }

        private void btnCallCenter_Click(object sender, EventArgs e)
        {
            ComboboxItem cbi = comboBoxCenters.SelectedItem as ComboboxItem;

            S_BowlingCenter bc = BowlingCenterManager.GetBowlingCenterById(Convert.ToInt16(cbi.Value));

            HMACAlgorithm hmac = new HMACAlgorithm(bc.appname, bc.secretkey);
            string authorisationHeader = hmac.GetAuthorizationHeader("GET", DateTime.UtcNow, bc.uri);
        }
    }
}
