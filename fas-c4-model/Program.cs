using Structurizr;
using Structurizr.Api;

namespace health_c4_model
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }
        
        static void Banking()
        {
            const long workspaceId = 69501;
            const string apiKey = "1ea7e361-555c-4eaf-a28c-3b3432fd3cfd";
            const string apiSecret = "1fa1caaf-8ca3-46a7-b321-a6bcb22aae0f";


            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("Health++ C4 Model - Sistema de salud", "Sistema de salud que enlaza paciente, médico y centro de salud");
            Model model = workspace.Model;

            SoftwareSystem healthSystem = model.AddSoftwareSystem(Location.Internal, "Health++", "Red que conecta a paciente, médico y centro de salud de  manera innovadora");
            SoftwareSystem notificactionSystem = model.AddSoftwareSystem(Location.Internal, "Notification System", "Sistema interno de notificaciones.");
            SoftwareSystem zoom = model.AddSoftwareSystem("Zoom", "Plataforma que ofrece una REST API de videollamadas.");

            Person patient = model.AddPerson("Paciente", "Persona que busca cuidar su salud.");
            Person doctor = model.AddPerson("Doctor", "Persona que atiende a los pacientes.");
            Person clinic = model.AddPerson("Centro de Salud (Pendiente a cambio de realción interna)", "Centro de atención médica.");
            

            patient.Uses(healthSystem, "Realiza reservas en los centros de salud y pide consultas con los médicos");
            doctor.Uses(healthSystem, "Brinda consultas a los pacientes y accede a su historial médico");
            clinic.Uses(healthSystem, "Gestiona las reservas y la información de sus pacientes");

            healthSystem.Uses(notificactionSystem, "Se comunica para la programación de una reunión");
            healthSystem.Uses(zoom, "Se comunica para la programación de una reunión");

            patient.Uses(notificactionSystem, "Recibe notificaciones a través de");
            //notificactionSystem.Uses(patient, "Comunica al paciente la reserva de una reunión");

            ViewSet viewSet = workspace.Views;


            //---------------------------//---------------------------//
            // 1. System Context Diagram
            //---------------------------//---------------------------//

            SystemContextView contextView = viewSet.CreateSystemContextView(healthSystem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A3_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();
            
            // Tags
            healthSystem.AddTags("SoftwareSystem");
            notificactionSystem.AddTags("NotificationSystem");
            zoom.AddTags("Zoom");
            patient.AddTags("Patient");
            doctor.AddTags("Doctor");
            clinic.AddTags("Clinic");
            
            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Patient") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Doctor") { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Clinic") { Background = "#facc2e", Shape = Shape.Robot });
            styles.Add(new ElementStyle("SoftwareSystem") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Zoom") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("NotificationSystem") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });


            //---------------------------//---------------------------//
            // 2. Conteiner Diagram
            //---------------------------//---------------------------//

            Container mobileApplication = healthSystem.AddContainer("Mobile App", "Permite a los usuarios visualizar un dashboard con las funcionalidades que brinda la aplicación.", "Android(Kotlin) y iOS(Swift)");
            Container webApplication = healthSystem.AddContainer("Web App", "Permite a los usuarios visualizar un dashboard con las funcionalidades que brinda la aplicación.", "Angular");
            Container landingPage = healthSystem.AddContainer("Landing Page", "", "HTML, CSS y JavaScript");

            Container apiGateway = healthSystem.AddContainer("API Gateway", "API Gateway", "Spring Boot port 8080");

            Container sessionContext = healthSystem.AddContainer("Session Bounded Context", "Bounded Context para gestión de sesiones", "Spring Boot port 8081");
            Container userContext = healthSystem.AddContainer("User Bounded Context", "Bounded Context para gestión de usuarios", "Spring Boot port 8081");
            Container bookingContext = healthSystem.AddContainer("Booking Bounded Context", "Bounded Context para gestión de reservas", "Spring Boot port 8081");
            Container diagnosisContext = healthSystem.AddContainer("Diagnosis Bounded Context", "Bounded Context para manejo de información del paciente", "Spring Boot port 8081");

            Container dataBase = healthSystem.AddContainer("Data Base", "", "SQL Server");
            
            
            patient.Uses(mobileApplication, "Consulta");
            patient.Uses(webApplication, "Consulta");
            patient.Uses(landingPage, "Consulta");
            
            doctor.Uses(mobileApplication, "Consulta");
            doctor.Uses(webApplication, "Consulta");
            doctor.Uses(landingPage, "Consulta");

            clinic.Uses(mobileApplication, "Consulta");
            clinic.Uses(webApplication, "Consulta");
            clinic.Uses(landingPage, "Consulta");

            mobileApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");
            
            apiGateway.Uses(sessionContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(userContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(bookingContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(diagnosisContext, "API Request", "JSON/HTTPS");

            sessionContext.Uses(dataBase, "", "JDBC");
            sessionContext.Uses(zoom, "Programa reunión", "JSON");

            userContext.Uses(dataBase, "", "JDBC");

            bookingContext.Uses(notificactionSystem, "Genera una notificación", "JDBC");
            bookingContext.Uses(dataBase, "", "JDBC");

            diagnosisContext.Uses(dataBase, "", "JDBC");

            //notificactionSystem.Uses(patient, "Envía notificación")
            zoom.Uses(sessionContext, "Retorna link de la reunión", "JSON");

            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiGateway.AddTags("APIGateway");

            sessionContext.AddTags("BoundedContext");
            userContext.AddTags("BoundedContext");
            bookingContext.AddTags("BoundedContext");
            diagnosisContext.AddTags("BoundedContext");

            dataBase.AddTags("DataBase");


            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIGateway") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            
            styles.Add(new ElementStyle("BoundedContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DataBase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });


            ContainerView containerView = viewSet.CreateContainerView(healthSystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();


            //---------------------------//---------------------------//
            // 3. Component Diagrams
            //---------------------------//---------------------------//

            // Components Diagram - Session Bounded Context
            Component scheduleController = sessionContext.AddComponent("Schedule Controller", "REST API endpoints de Schedule", "Spring Boot REST Controller");
            Component sessionDetailsController = sessionContext.AddComponent("Session Details Controller", "REST API endpoints de Session Details", "Spring Boot REST Controller");
            Component sessionsController = sessionContext.AddComponent("Sessions Controller", "REST API endpoints de Session", "Spring Boot REST Controller");

            Component scheduleService = sessionContext.AddComponent("Schedule Service", "Provee métodos para Schedule", "Spring Component");
            Component sessionDetailService = sessionContext.AddComponent("Session Detail Service", "Provee métodos para Schedule Detail", "Spring Component");
            Component sessionService = sessionContext.AddComponent("Session Service", "Provee métodos para Session", "Spring Component");

            Component scheduleRepository = sessionContext.AddComponent("Schedule Repository", "Provee los métodos para la persistencia de datos de Schedule", "Spring Component");
            Component sessionDetailRepository = sessionContext.AddComponent("Session Details Repository", "Provee los métodos para la persistencia de datos de Session Detail", "Spring Component");
            Component sessionRepository = sessionContext.AddComponent("Session Repository", "Provee los métodos para la persistencia de datos de Session", "Spring Component");

            Component zoomController = sessionContext.AddComponent("Zoom Controller", "REST API ", "Spring Boot REST Controller");
            Component zoomFacade = sessionContext.AddComponent("Zoom Facade", "", "Spring Component");


            // Components Diagram - User Bounded Context
            Component rolesController = userContext.AddComponent("Roles Controller", "REST API endpoints de Roles", "Spring Boot REST Controller");
            Component userController = userContext.AddComponent("User Controller", "REST API endpoints de User", "Spring Boot REST Controller");
            Component userScheduleController = userContext.AddComponent("user Schedule Controller", "REST API endpoints de User Schedule", "Spring Boot REST Controller");

            Component roleService = userContext.AddComponent("Role Service", "Provee métodos para Role", "Spring Component");
            Component userService = userContext.AddComponent("User Service", "Provee métodos para User", "Spring Component");
            Component userScheduleService = userContext.AddComponent("User Schedule Service", "Provee métodos para User Schedule", "Spring Component");

            Component roleRepository = userContext.AddComponent("Role Repository", "Provee los métodos para la persistencia de datos de Role", "Spring Component");
            Component userRepository = userContext.AddComponent("User Repository", "Provee los métodos para la persistencia de datos de Laguage of User", "Spring Component");
            Component userScheduleRepository = userContext.AddComponent("User Schedule Repository", "Provee los métodos para la persistencia de datos de User Schedule", "Spring Component");


            // Components Diagram - Booking Bounded Context
            Component bookingController = bookingContext.AddComponent("Booking Controller", "REST API endpoints de Booking", "Spring Boot REST Controller");
            Component notificationComponent = bookingContext.AddComponent("Notification Component", "Envía notificaciones a Pacientes", "Spring Boot REST Controller");

            Component bookingService = bookingContext.AddComponent("Booking Service", "Provee métodos para Booking", "Spring Component");

            Component bookingRepository = bookingContext.AddComponent("Booking Repository", "Provee los métodos para la persistencia de datos de Reserva", "Spring Component");
            Component notificationRepository = bookingContext.AddComponent("Notification Repository", "Provee los métodos para la persistencia de datos de Notificaciones", "Spring Component");


            // Components Diagram - Diagnosis Bounded Context
            Component diagnosisController = diagnosisContext.AddComponent("Diagnosis Controller", "REST API endpoints de Diagnósticos", "Spring Boot REST Controller");
            Component userProfileController = diagnosisContext.AddComponent("User Profile Controller", "REST API endpoints de User Profile", "Spring Boot REST Controller");

            Component diagnosisService = diagnosisContext.AddComponent("Diagnosis Service", "Provee métodos para Diagnósticos", "Spring Component");
            Component userProfileService = diagnosisContext.AddComponent("User Profile Service", "Provee métodos para User Profile", "Spring Component");

            Component diagnosisRepository = diagnosisContext.AddComponent("Diagnosis Repository", "Provee los métodos para la persistencia de datos de Diagnósticos", "Spring Component");



            // Tags
            bookingController.AddTags("Controller");
            bookingService.AddTags("Service");
            bookingRepository.AddTags("Repository");

            notificationComponent.AddTags("Controller");
            notificationRepository.AddTags("Repository");

            rolesController.AddTags("Controller");
            roleService.AddTags("Service");
            roleRepository.AddTags("Repository");

            scheduleController.AddTags("Controller");
            scheduleService.AddTags("Service");
            scheduleRepository.AddTags("Repository");

            sessionDetailsController.AddTags("Controller");
            sessionDetailService.AddTags("Service");
            sessionDetailRepository.AddTags("Repository");

            sessionsController.AddTags("Controller");
            sessionService.AddTags("Service");
            sessionRepository.AddTags("Repository");

            bookingRepository.AddTags("Repository");

            userController.AddTags("Controller");
            userService.AddTags("Service");
            userRepository.AddTags("Repository");

            userScheduleController.AddTags("Controller");
            userScheduleService.AddTags("Service");
            userScheduleRepository.AddTags("Repository");

            diagnosisController.AddTags("Controller");
            diagnosisService.AddTags("Service");
            diagnosisRepository.AddTags("Repository");

            userProfileController.AddTags("Controller");
            userProfileService.AddTags("Service");

            zoomController.AddTags("Controller");
            zoomFacade.AddTags("Service");


            styles.Add(new ElementStyle("Controller") { Shape = Shape.Component, Background = "#FDFF8B", Icon = "" });
            styles.Add(new ElementStyle("Service") { Shape = Shape.Component, Background = "#FEF535", Icon = "" });
            styles.Add(new ElementStyle("Repository") { Shape = Shape.Component, Background = "#FFC100", Icon = "" });




            //Component connection: Booking
            apiGateway.Uses(bookingController, "", "JSON/HTTPS");
            bookingController.Uses(bookingService, "Llama a los métodos del Service");
            bookingService.Uses(bookingRepository, "Usa");
            /**/bookingService.Uses(notificationComponent, "Usa");
            /**/bookingService.Uses(notificationRepository, "Lee desde y escribe hasta");
            bookingRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Notification
            notificationComponent.Uses(notificationRepository, "Usa");
            notificationComponent.Uses(notificactionSystem, "Comunica a través de");
            notificationRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Role 
            apiGateway.Uses(rolesController, "", "JSON/HTTPS");
            rolesController.Uses(roleService, "Llama a los métodos del Service");
            roleService.Uses(roleRepository, "Usa");
            roleRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Schedule 
            apiGateway.Uses(scheduleController, "", "JSON/HTTPS");
            scheduleController.Uses(scheduleService, "Llama a los métodos del Service");
            scheduleService.Uses(scheduleRepository, "Usa");
            scheduleRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Session Details 
            apiGateway.Uses(sessionDetailsController, "", "JSON/HTTPS");
            sessionDetailsController.Uses(sessionDetailService, "Llama a los métodos del Service");
            sessionDetailService.Uses(sessionDetailRepository, "Usa");
            /**/sessionDetailService.Uses(sessionRepository, "Usa");
            sessionDetailRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Session 
            apiGateway.Uses(sessionsController, "", "JSON/HTTPS");
            sessionsController.Uses(sessionService, "Llama a los métodos del Service");
            sessionService.Uses(sessionRepository, "Usa");
            /**/sessionService.Uses(userScheduleRepository, "Usa");
            sessionRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: User 
            apiGateway.Uses(userController, "", "JSON/HTTPS");
            userController.Uses(userService, "Llama a los métodos del Service");
            userService.Uses(userRepository, "Usa");
            /**/userService.Uses(roleRepository, "Usa");
            userRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: User Schedule
            apiGateway.Uses(userScheduleController, "", "JSON/HTTPS");
            userScheduleController.Uses(userScheduleService, "Llama a los métodos del Service");
            userScheduleService.Uses(userScheduleRepository, "Usa");
            /**/userScheduleService.Uses(userRepository, "Usa");
            /**/userScheduleService.Uses(scheduleRepository, "Usa");
            userScheduleRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Diagnosis 
            apiGateway.Uses(diagnosisController, "", "JSON/HTTPS");
            diagnosisController.Uses(diagnosisService, "Llama a los métodos del Service");
            diagnosisService.Uses(diagnosisRepository, "Usa");
            diagnosisRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: User Profile 
            apiGateway.Uses(userProfileController, "", "JSON/HTTPS");
            userProfileController.Uses(userProfileService, "Llama a los métodos del Service");
            userProfileService.Uses(userRepository, "Usa");
            //userRepository.Uses(dataBase, "Lee desde y escribe hasta");


            //Component connection: External
            //Zoom
            apiGateway.Uses(zoomController, "", "JSON/HTTPS");
            zoomController.Uses(zoomFacade, "Llama a los métodos del Service");
            zoomFacade.Uses(zoom, "Usa");



            // View - Components Diagram - Session Bounded Context
            ComponentView sessionComponentView = viewSet.CreateComponentView(sessionContext, "Session Bounded Context's Components", "Component Diagram");
            sessionComponentView.PaperSize = PaperSize.A3_Landscape;
            sessionComponentView.Add(mobileApplication);
            sessionComponentView.Add(webApplication);
            sessionComponentView.Add(apiGateway);
            sessionComponentView.Add(dataBase);
            sessionComponentView.Add(zoom);
            sessionComponentView.AddAllComponents();

            // View - Components Diagram - User Bounded Context
            ComponentView userComponentView = viewSet.CreateComponentView(userContext, "User Bounded Context's Components", "Component Diagram");
            userComponentView.PaperSize = PaperSize.A3_Landscape;
            userComponentView.Add(mobileApplication);
            userComponentView.Add(webApplication);
            userComponentView.Add(apiGateway);
            userComponentView.Add(dataBase);
            userComponentView.AddAllComponents();

            // View - Components Diagram - Booking Bounded Context
            ComponentView bookingComponentView = viewSet.CreateComponentView(bookingContext, "Booking Bounded Context's Components", "Component Diagram");
            bookingComponentView.PaperSize = PaperSize.A3_Landscape;
            bookingComponentView.Add(mobileApplication);
            bookingComponentView.Add(webApplication);
            bookingComponentView.Add(apiGateway);
            bookingComponentView.Add(dataBase);
            bookingComponentView.Add(notificactionSystem);
            bookingComponentView.AddAllComponents();

            // View - Components Diagram - Diagnosis Bounded Context
            ComponentView diagnosisComponentView = viewSet.CreateComponentView(diagnosisContext, "Diagnosis Bounded Context's Components", "Component Diagram");
            diagnosisComponentView.PaperSize = PaperSize.A3_Landscape;
            diagnosisComponentView.Add(clinic);
            diagnosisComponentView.Add(doctor);
            diagnosisComponentView.Add(mobileApplication);
            diagnosisComponentView.Add(webApplication);
            diagnosisComponentView.Add(apiGateway);
            diagnosisComponentView.Add(dataBase);
            diagnosisComponentView.Add(userRepository);
            diagnosisComponentView.AddAllComponents();



            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}