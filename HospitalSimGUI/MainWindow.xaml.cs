using Core.AgentOrientedSimulation.Models.DTO;
using OSPAnimator;
using Simulation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using WPFComponentsLibrary;

namespace HospitalSimGUI
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<DoctorDTO> DoctorsList { get; set; } = new();
        public ObservableCollection<NurseDTO> NursesList { get; set; } = new();
        public ObservableCollection<AmbulanceADTO> AmbulanceAList { get; set; } = new();
        public ObservableCollection<AmbulanceBDTO> AmbulanceBList { get; set; } = new();

        public MySimulation Sim { get; set; } = new MySimulation();

        public MainWindow()
        {
            InitializeComponent();            

            dgDoctors.ItemsSource = DoctorsList;
            dgNurses.ItemsSource = NursesList;
            dgAmbulanceA.ItemsSource = AmbulanceAList;
            dgAmbulanceB.ItemsSource = AmbulanceBList;

            TopBar.OnSpeedChangeRequest += HandleSpeedChange;

            topBar.DisableButtons();
        }

        private void InitRooms()
        {
            AnimItem waitingRoom = new AnimShapeItem(AnimShape.RECTANGLE, Colors.LightGray, 100)
            {
                Width = 200,
                Height = 150
            };
            waitingRoom.SetPosition(100, 50);
            waitingRoom.ZIndex = 0;

            AnimTextItem waitingRoomLabel = new AnimTextItem("Waiting Room");
            waitingRoomLabel.SetPosition(100, 200);
            waitingRoomLabel.ZIndex = 0;

            AnimItem waitingRoom2 = new AnimShapeItem(AnimShape.RECTANGLE, Colors.LightYellow, 100)
            {
                Width = 200,
                Height = 150
            };
            waitingRoom2.SetPosition(100, 250);
            waitingRoom2.ZIndex = 0;

            AnimTextItem waitingRoomLabel2 = new AnimTextItem("Waiting Room2");
            waitingRoomLabel2.SetPosition(100, 400);
            waitingRoomLabel2.ZIndex = 0;

            AnimItem entrance = new AnimShapeItem(AnimShape.RECTANGLE, Colors.LightGreen, 100)
            {
                Width = 50,
                Height = 50
            };
            entrance.SetPosition(50, 50);
            entrance.ZIndex = 0;

            AnimTextItem entranceLabel = new AnimTextItem("Entrance");
            entranceLabel.SetPosition(50, 110);
            entranceLabel.ZIndex = 0;

            AnimItem exit = new AnimShapeItem(AnimShape.RECTANGLE, Colors.LightCoral, 100)
            {
                Width = 50,
                Height = 50
            };
            exit.SetPosition(800, 50);
            exit.ZIndex = 0;

            AnimTextItem exitLabel = new AnimTextItem("Exit");
            exitLabel.SetPosition(800, 110);
            exitLabel.ZIndex = 0;

            Sim.Animator.Register(waitingRoom);
            Sim.Animator.Register(waitingRoomLabel);
            Sim.Animator.Register(waitingRoom2);
            Sim.Animator.Register(waitingRoomLabel2);
            Sim.Animator.Register(entrance);
            Sim.Animator.Register(entranceLabel);
            Sim.Animator.Register(exit);
            Sim.Animator.Register(exitLabel);
        }

        private void InitResources()
        {
            if (Sim.AgentDoctor?.Doctors != null)
            {
                foreach (var doc in Sim.AgentDoctor.Doctors)
                {
                    DoctorsList.Add(DTOManager.MapToDoctorDTO(doc));
                }
            }

            if (Sim.AgentNurse?.Nurses != null)
            {
                foreach (var nurse in Sim.AgentNurse.Nurses)
                {
                    NursesList.Add(DTOManager.MapToNurseDTO(nurse));
                }
            }

            if (Sim.AgentAmbulance?.AmbulancesA != null)
            {
                foreach (var ambA in Sim.AgentAmbulance.AmbulancesA)
                {
                    AmbulanceAList.Add(DTOManager.MapToAmbulanceADTO(ambA));
                }
            }

            if (Sim.AgentAmbulance?.AmbulancesB != null)
            {
                foreach (var ambB in Sim.AgentAmbulance.AmbulancesB)
                {
                    AmbulanceBList.Add(DTOManager.MapToAmbulanceBDTO(ambB));
                }
            }
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            viewStatistics.Visibility = Visibility.Visible;
            viewAnimation.Visibility = Visibility.Collapsed;
        }

        private void BtnAnimation_Click(object sender, RoutedEventArgs e)
        {
            viewStatistics.Visibility = Visibility.Collapsed;
            viewAnimation.Visibility = Visibility.Visible;
        }

        private void UpdateStatistics()
        {
            txtTime.Text = TimeSpan.FromSeconds(Sim.CurrentTime).ToString(@"dd\.hh\:mm\:ss");

            txtStatistics.Items.Clear();

            if (!Sim.IsTurboMode)
            {
                int waitingCount = Sim.AgentOfUrgent.AmbulancesInitialPatientQueue.Count;
                double averageQueueLengthWalkIn = Sim.AgentOfUrgent.PatientWalkInInitExamQueueStat.AverageQueueLength;
                double averageQueueLengthAmbulance = Sim.AgentOfUrgent.PatientAmbulanceInitExamQueueStat.AverageQueueLength;
                int waitingForExamCount = Sim.AgentOfUrgent.AmbulancesExamPatientQueue.Count;
                double averageExamQueueLengthWalkIn = Sim.AgentOfUrgent.PatientWalkInExaminationQueueStat.AverageQueueLength;
                double averageExamQueueLengthAmbulance = Sim.AgentOfUrgent.PatientAmbulanceExaminationQueueStat.AverageQueueLength;

                txtStatistics.Items.Add($"Total Patients (walk): {Sim.AgentEnv.TotalPatientsWalk}");
                txtStatistics.Items.Add($"Total Patients (ambulance): {Sim.AgentEnv.TotalPatientsAmbulance}");
                txtStatistics.Items.Add($"Total Patients (all): {Sim.AgentEnv.TotalPatientsAmbulance + Sim.AgentEnv.TotalPatientsWalk}");

                double avgWalk = double.IsNaN(Sim.AgentEnv.ReplicationTimeInSystemWalk.Mean) ? 0 : Sim.AgentEnv.ReplicationTimeInSystemWalk.Mean;
                double avgAmb = double.IsNaN(Sim.AgentEnv.ReplicationTimeInSystemAmbulance.Mean) ? 0 : Sim.AgentEnv.ReplicationTimeInSystemAmbulance.Mean;

                txtStatistics.Items.Add($"Average Time in System Patients (walk): {TimeSpan.FromSeconds(avgWalk):mm\\:ss}");
                txtStatistics.Items.Add($"Average Time in System Patients (ambulance): {TimeSpan.FromSeconds(avgAmb):mm\\:ss}");

                txtStatistics.Items.Add($"Patients Waiting for Initial Exam: {waitingCount}");
                txtStatistics.Items.Add($"Average Queue Length for Initial Exam (Walk In): {averageQueueLengthWalkIn}");
                txtStatistics.Items.Add($"Average Queue Length for Initial Exam (Ambulance): {averageQueueLengthAmbulance}");
                txtStatistics.Items.Add($"Patients Waiting for Examination: {waitingForExamCount}");
                txtStatistics.Items.Add($"Average Queue Length for Examination (Walk In): {averageExamQueueLengthWalkIn}");
                txtStatistics.Items.Add($"Average Queue Length for Examination (Ambulance): {averageExamQueueLengthAmbulance}");

                double simTime = Sim.CurrentTime;

                double utilNurses = Sim.AgentNurse.BusyNurseStat.Mean / Sim.CountOfNurses;
                double utilDoctors = Sim.AgentDoctor.BusyDoctorStat.Mean / Sim.CountOfDoctors;
                double utilAmbA = Sim.AgentAmbulance.BusyAmbulanceAStat.Mean / Sim.CountOfAmbulancesA;
                double utilAmbB = Sim.AgentAmbulance.BusyAmbulanceBStat.Mean / Sim.CountOfAmbulancesB;

                txtStatistics.Items.Add($"Utilization (Nurses): {utilNurses:P2}");
                txtStatistics.Items.Add($"Utilization (Doctors): {utilDoctors:P2}");
                txtStatistics.Items.Add($"Utilization (Ambulance A): {utilAmbA:P2}");
                txtStatistics.Items.Add($"Utilization (Ambulance B): {utilAmbB:P2}");

                RefreshDTOTables();
            }
            else
            {
                if (Sim.CurrentReplication > 0)
                {
                    txtActualReplication.Text = Sim.CurrentReplication.ToString();

                    
                    var totalPatientsWalkCI = Sim.GlobalPatientsWalkIn.ConfidenceInterval95();
                    var totalPatientsAmbulanceCI = Sim.GlobalPatientsAmbulance.ConfidenceInterval95();
                    var totalPatientsAllCI = Sim.GlobalPatientsAll.ConfidenceInterval95();

                    if (double.IsNaN(totalPatientsWalkCI.Item1) && double.IsNaN(totalPatientsWalkCI.Item2))
                    {
                        totalPatientsWalkCI = (0, 0);
                    }
                    if (double.IsNaN(totalPatientsAmbulanceCI.Item1) && double.IsNaN(totalPatientsAmbulanceCI.Item2))
                    {
                        totalPatientsAmbulanceCI = (0, 0);
                    }
                    if (double.IsNaN(totalPatientsAllCI.Item1) && double.IsNaN(totalPatientsAllCI.Item2))
                    {
                        totalPatientsAllCI = (0, 0);
                    }

                    txtStatistics.Items.Add($"Total Patients (walk): {Sim.GlobalPatientsWalkIn.Mean}" +
                        $"<{totalPatientsWalkCI.Item1:F2} ; {totalPatientsWalkCI.Item2:F2}>");
                    txtStatistics.Items.Add($"Total Patients (ambulance): {Sim.GlobalPatientsAmbulance.Mean}" +
                        $"<{totalPatientsAmbulanceCI.Item1:F2} ; {totalPatientsAmbulanceCI.Item2:F2}>");
                    txtStatistics.Items.Add($"Total Patients (all): {Sim.GlobalPatientsAll.Mean}" +
                        $"<{totalPatientsAllCI.Item1:F2} ; {totalPatientsAllCI.Item2:F2}>");

                    var timeInSystemWalkCI = Sim.GlobalTimeInSystemWalk.ConfidenceInterval95();
                    var timeInSystemAmbulanceCI = Sim.GlobalTimeInSystemAmbulance.ConfidenceInterval95();

                    if (double.IsNaN(timeInSystemWalkCI.Item1) && double.IsNaN(timeInSystemWalkCI.Item2))
                    {
                        timeInSystemWalkCI = (0, 0);
                    }
                    if (double.IsNaN(timeInSystemAmbulanceCI.Item1) && double.IsNaN(timeInSystemAmbulanceCI.Item2))
                    {
                        timeInSystemAmbulanceCI = (0, 0);
                    }

                    txtStatistics.Items.Add($"Average Time in System Patients (walk): {TimeSpan.FromSeconds(Sim.GlobalTimeInSystemWalk.Mean):mm\\:ss}" +
                        $"<{TimeSpan.FromSeconds(timeInSystemWalkCI.Item1):mm\\:ss} ; {TimeSpan.FromSeconds(timeInSystemWalkCI.Item2):mm\\:ss}>");
                    txtStatistics.Items.Add($"Average Time in System Patients (ambulance): {TimeSpan.FromSeconds(Sim.GlobalTimeInSystemAmbulance.Mean):mm\\:ss}" +
                        $"<{TimeSpan.FromSeconds(timeInSystemAmbulanceCI.Item1):mm\\:ss} ; {TimeSpan.FromSeconds(timeInSystemAmbulanceCI.Item2):mm\\:ss}>");

                    var initExamWalkInCI = Sim.GlobalInitialExamWaitWalkIn.ConfidenceInterval95();
                    var initExamAmbulanceCI = Sim.GlobalInitialExamWaitAmbulance.ConfidenceInterval95();
                    var examWaitWalkInCI = Sim.GlobalExaminationWaitWalkIn.ConfidenceInterval95();
                    var examWaitAmbulanceCI = Sim.GlobalExaminationWaitAmbulance.ConfidenceInterval95();

                    if (double.IsNaN(initExamWalkInCI.Item1) && double.IsNaN(initExamWalkInCI.Item2))
                    {
                        initExamWalkInCI = (0, 0);
                    }
                    if (double.IsNaN(initExamAmbulanceCI.Item1) && double.IsNaN(initExamAmbulanceCI.Item2))
                    {
                        initExamAmbulanceCI = (0, 0);
                    }
                    if (double.IsNaN(examWaitWalkInCI.Item1) && double.IsNaN(examWaitWalkInCI.Item2))
                    {
                        examWaitWalkInCI = (0, 0);
                    }
                    if (double.IsNaN(examWaitAmbulanceCI.Item1) && double.IsNaN(examWaitAmbulanceCI.Item2))
                    {
                        examWaitAmbulanceCI = (0, 0);
                    }

                    txtStatistics.Items.Add($"Initial exam wait (Walk In): " +
                        $"{TimeSpan.FromSeconds(Sim.GlobalInitialExamWaitWalkIn.Mean):hh\\:mm\\:ss}, " +
                        $"<{TimeSpan.FromSeconds(initExamWalkInCI.Item1):hh\\:mm\\:ss} ; {TimeSpan.FromSeconds(initExamWalkInCI.Item2):hh\\:mm\\:ss}>");
                    txtStatistics.Items.Add($"Initial exam wait (Ambulance): " +
                        $"{TimeSpan.FromSeconds(Sim.GlobalInitialExamWaitAmbulance.Mean):hh\\:mm\\:ss}, " +
                        $"<{TimeSpan.FromSeconds(initExamAmbulanceCI.Item1):hh\\:mm\\:ss} ; {TimeSpan.FromSeconds(initExamAmbulanceCI.Item2):hh\\:mm\\:ss}>");
                    txtStatistics.Items.Add($"Examination wait (Walk In): " +
                        $"{TimeSpan.FromSeconds(Sim.GlobalExaminationWaitWalkIn.Mean):hh\\:mm\\:ss}, " +
                        $"<{TimeSpan.FromSeconds(examWaitWalkInCI.Item1):hh\\:mm\\:ss} ; {TimeSpan.FromSeconds(examWaitWalkInCI.Item2):hh\\:mm\\:ss}>");
                    txtStatistics.Items.Add($"Examination wait (Ambulance): " +
                        $"{TimeSpan.FromSeconds(Sim.GlobalExaminationWaitAmbulance.Mean):hh\\:mm\\:ss}, " +
                        $"<{TimeSpan.FromSeconds(examWaitAmbulanceCI.Item1):hh\\:mm\\:ss} ; {TimeSpan.FromSeconds(examWaitAmbulanceCI.Item2):hh\\:mm\\:ss}>");


                    var nursesUtilCI = Sim.GlobalUtilizationNurses.ConfidenceInterval95();
                    var doctorsUtilCI = Sim.GlobalUtilizationDoctors.ConfidenceInterval95();
                    var ambAUtilCI = Sim.GlobalUtilizationAmbulanceA.ConfidenceInterval95();
                    var ambBUtilCI = Sim.GlobalUtilizationAmbulanceB.ConfidenceInterval95();

                    if (double.IsNaN(nursesUtilCI.Item1) && double.IsNaN(nursesUtilCI.Item2))
                    {
                        nursesUtilCI = (0, 0);
                    }
                    if (double.IsNaN(doctorsUtilCI.Item1) && double.IsNaN(doctorsUtilCI.Item2))
                    {
                        doctorsUtilCI = (0, 0);
                    }
                    if (double.IsNaN(ambAUtilCI.Item1) && double.IsNaN(ambAUtilCI.Item2))
                    {
                        ambAUtilCI = (0, 0);
                    }
                    if (double.IsNaN(ambBUtilCI.Item1) && double.IsNaN(ambBUtilCI.Item2))
                    {
                        ambBUtilCI = (0, 0);
                    }

                    txtStatistics.Items.Add($"utilization (Nurses): {Sim.GlobalUtilizationNurses.Mean:P2}" +
                        $"<{nursesUtilCI.Item1:P2} ; {nursesUtilCI.Item2:P2}>");
                    txtStatistics.Items.Add($"utilization (Doctors): {Sim.GlobalUtilizationDoctors.Mean:P2}" +
                        $"<{doctorsUtilCI.Item1:P2} ; {doctorsUtilCI.Item2:P2}>");
                    txtStatistics.Items.Add($"utilization (Amb. A): {Sim.GlobalUtilizationAmbulanceA.Mean:P2}" +
                        $"<{ambAUtilCI.Item1:P2} ; {ambAUtilCI.Item2:P2}>");
                    txtStatistics.Items.Add($"utilization (Amb. B): {Sim.GlobalUtilizationAmbulanceB.Mean:P2}" +
                        $"<{ambBUtilCI.Item1:P2} ; {ambBUtilCI.Item2:P2}>");

                    var initExamQueueCI = Sim.GlobalInitialExamAvgQueueStat.ConfidenceInterval95();
                    var examQueueCI = Sim.GlobalExamAvgQueueStat.ConfidenceInterval95();

                    txtStatistics.Items.Add($"Queue length (Initial exam): {Sim.GlobalInitialExamAvgQueueStat.Mean:F2}" +
                        $"<{initExamQueueCI.Item1:F2} ; {initExamQueueCI.Item2:F2}>");
                    txtStatistics.Items.Add($"Queue length (Examination): {Sim.GlobalExamAvgQueueStat.Mean:F2}" +
                        $"<{examQueueCI.Item1:F2} ; {examQueueCI.Item2:F2}>");
                }
            } 
        }
         private void RefreshDTOTables()
        {
            DoctorsList.Clear();
            NursesList.Clear();
            AmbulanceAList.Clear();
            AmbulanceBList.Clear();
            if (Sim.AgentDoctor?.Doctors != null)
            {
                foreach (var doc in Sim.AgentDoctor.Doctors)
                    DoctorsList.Add(DTOManager.MapToDoctorDTO(doc));
            }

            if (Sim.AgentNurse?.Nurses != null)
            {
                foreach (var nurse in Sim.AgentNurse.Nurses)
                    NursesList.Add(DTOManager.MapToNurseDTO(nurse));
            }

            if (Sim.AgentAmbulance?.AmbulancesA != null)
            {
                foreach (var ambA in Sim.AgentAmbulance.AmbulancesA)
                    AmbulanceAList.Add(DTOManager.MapToAmbulanceADTO(ambA));
            }

            if (Sim.AgentAmbulance?.AmbulancesB != null)
            {
                foreach (var ambB in Sim.AgentAmbulance.AmbulancesB)
                    AmbulanceBList.Add(DTOManager.MapToAmbulanceBDTO(ambB));
            } 
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Sim.IsTurboMode = (checkTurboMode.IsChecked == true);

            if (Sim.IsTurboMode)
            {
                Sim.OnReplicationFinishedAction = () =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        UpdateStatistics();
                    });
                };
            } else
            {
                Sim.OnRefreshUI(sim =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        UpdateStatistics();
                    });
                });
            }

            Sim.CountOfNurses = int.TryParse(txtNurses.Text, out int n) ? n : 5;
            Sim.CountOfDoctors = int.TryParse(txtDoctors.Text, out int d) ? d : 3;
            int replications = int.TryParse(topBar.Replications, out int r) ? r : 10;

            double simTime = double.TryParse(txtSimTime.Text, out double t) ? t : 28 * 24 * 60 * 60;
            double warmupTime = double.TryParse(txtWarmUpTime.Text, out double w) ? w : 20 * 24 * 60 * 60;

            Sim.WarmUpTime = warmupTime;

            InitResources();
            if (!Sim.IsTurboMode)
            {
                btnAnimation.IsEnabled = true;
                Sim.CreateAnimator();
                animationCanvas.Child = Sim.Animator.Canvas;
                InitRooms();
            }

            Task.Run(() =>
            {
                if (Sim.IsTurboMode)
                {
                    Sim.SetMaxSimSpeed();
                } else
                {
                    HandleSpeedChange();
                }
                Sim.Simulate(replications, simTime + warmupTime);

                Dispatcher.Invoke(() =>
                {
                    UpdateStatistics();
                });
            });
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            Sim.PauseSimulation();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Sim.StopSimulation();
        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            Sim.ResumeSimulation();
        }
        private void HandleSpeedChange()
        {
            double baseDuration = 0.5;
            double newDuration = baseDuration / topBar.TimeMultiplierValue;

            Sim.SetSimSpeed(10, newDuration);
        }

        private void btnVariant1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnVariant2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnVariant3_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void btnCreateAnimator_Click(object sender, RoutedEventArgs e)
        //{
        //    btnAnimation.IsEnabled = true;
        //    Sim.CreateAnimator();
        //    animationCanvas.Child = Sim.Animator.Canvas;            
        //}
        //private void btnDeleteAnimator_Click(object sender, RoutedEventArgs e)
        //{
        //    btnAnimation.IsEnabled = false;
        //    Sim.RemoveAnimator();
        //    animationCanvas.Child = null;

        //}
    }
}