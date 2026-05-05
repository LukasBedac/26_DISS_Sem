using Core.AgentOrientedSimulation.Models.DTO;
using OSPAnimator;
using Simulation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

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

            btnAnimation.IsEnabled = true;
            Sim.CreateAnimator();
            animationCanvas.Child = Sim.Animator.Canvas;
            InitRooms();

            dgDoctors.ItemsSource = DoctorsList;
            dgNurses.ItemsSource = NursesList;
            dgAmbulanceA.ItemsSource = AmbulanceAList;
            dgAmbulanceB.ItemsSource = AmbulanceBList;

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

            int waitingCount = Sim.AgentOfUrgent.AmbulancesInitialPatientQueue.Count;
            double averageQueueLength = Sim.AgentOfUrgent.AmbulancesInitialStatistic.AverageQueueLength;

            txtStatistics.Items.Add($"=== CURRENT STATE ===");
            txtStatistics.Items.Add($"Patients Waiting for Initial Exam: {waitingCount}");
            txtStatistics.Items.Add($"Average Queue Length for Initial Exam: {averageQueueLength}");

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

            txtLogs.Items.Clear();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Sim.OnRefreshUI(s =>
            {
                Dispatcher.Invoke(() =>
                {
                    UpdateStatistics();
                });
            });

            Sim.CountOfNurses = int.TryParse(txtNurses.Text, out int n) ? n : 5;
            Sim.CountOfDoctors = int.TryParse(txtDoctors.Text, out int d) ? d : 3;

            InitResources();
            Task.Run(() =>
            {
                Sim.SetSimSpeed(10, 0.5);
                Sim.Simulate(1, 2419200);
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