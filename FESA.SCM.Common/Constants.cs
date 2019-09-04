namespace FESA.SCM.Common {
	public class Constants {
		public const string EncryptionKey = "F354@Azur3";

		public static class MailBodies {
			public const string Traveling = "<!Doctype html>" +
"<html>" +
	"<head>" +
		"<meta charset=utf-8>" +
	"</head>" +

	"<body style=\"font-family: sans-serif;\">" +
		"<section style=\"with:690px;\">" +
			"<div>" +
				  "<img src=" + "{11}" + ">" +
			"</div>" +
			"<div  style=\"background:rgb(255,194,15);text-align:right;\">" +
				  "<span>" + "{0}" + "</span>" +
			"</div>" +
		   " <h3>" +
				"Estimado Cliente," +
			"</h3>" +
			"<p>" +
				"Hemos iniciado nuestro viaje a sus instalaciones en" +
				"<span style=\" color:rgb(180, 74, 109);font-weight: 600;\">" +
					"{1} " + "</span>" +
				"para atender el" + "<br>" +
				"servicio" +
				"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {2} " + "</span>" +
				"solicitado" + "<br>" + "para el equipo modelo" + "<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {3} " +
				"</span>" +
				"serie" +
				"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {4} " + "</span>" +
			"</p>" +
			"<div>" +
				"<p>" +
					"El Personal asignado para este servicio es el siguiente:" +
				"</p>" +

				"<ul style=\"font-weight: 600;\">" +
					"{5}" +
				//"<li> {5} </li>" +
				//"<li> {6} </li>" +
				"</ul>" +
			"</div>" +
			"<div>" +
				"<p>" +
					"Atentamente," +
				"</p>" +
				"<p>" +
					"<span style=\"color:rgb(180, 74, 109\">" + "{6}" + "</span>" + "<br>" + "<span>" + "Mecánico Líder asignado" + "</span>" +
				"</p>" +
			"</div>" +
			"<div>" +
				"<p>" +
					"RPM de contacto: <strong>{7}</strong>" + "<br>" +
					"Correo electrónico: " + "<span style=\" text-decoration: underline;color: blue;\">" + "{8}" +
					"</span>" +
				"</p>" +
			"</div>" +
			"<div style=\" padding: 10px;background: #f8f8f8;\">" +
				"<p>" +
					"<strong>Nota:</strong> Cualquier inconveniente o consulta adicional comunicarse con el supervisor" + "<br>" +
					"<span  style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {9} " + "</span>" + "o escríbale a " +
					"<span style=\"text-decoration: underline;color: blue;\">" + "{10} " +
					"</span>" +
				"</p>" +
			"</div>" +
	   "</section>" +
	"</body>" +
"</html>";
			public const string Driving = "<!Doctype html>" +
"<html>" +
	"<head>" +
		"<meta charset=utf-8>" +
	"</head>" +
	"<body style=\"font-family: sans-serif;\">" +
		"<section style=\"with:690px;\">" +
			"<div>" +
				  "<img src=" + "{13}" + ">" +
			"</div>" +
			"<div  style=\"background:rgb(255,194,15);text-align:right;\">" +
				  "<span>" + "{0}" + "</span>" +
			"</div>" +
		   " <h3>" +
				"Estimado Cliente," +
			"</h3>" +
			"<p>" +
				"Hemos iniciado nuestro viaje a sus instalaciones en" +
				"<span  style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + "{1} " + "</span>" + "para atender el servicio" + "<br>" +
				"<span  style=\" color:rgb(180, 74, 109);font-weight: 600;\">" + "{2} " + "</span>" + "solicitado para el equipo modelo " +
				"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + "{3} " + "</span>" + "serie" + "<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {4} " + "</span>" +
			"</p>" +
			"<div>" +
				"<p>" +
					"El Personal asignado para este servicio es el siguiente:" +
				"</p>" +
				"<ul style=\"font-weight: 600;\">" +
				"{5}" +
				"</ul>" +
			"</div>" +
			"<div>" +
				"<p>" +
					"Atentamente," +
				"</p>" +
				"<p>" +
					"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + "{6}" + "</span>" + "<br>" +
					"<span>" + "Mecánico Líder asignado" + "</span>" + "</p>" +
			"</div>" +
			"<div>" +
				"<p>" +
					"Teléfono de contacto : <strong> {7} </strong>" + "<br>" +
					"RPM de contacto : <strong> {8} </strong>" + "<br>" +
					"Correo electrónico: " + "<span style=\" text-decoration: underline;color: blue;\">" +
					"{9}" + "</span>" + "</p>" +
			"</div>" +
			"<div style=\" padding: 10px;background: #f8f8f8;\">" +
				"<p>" +
					"<strong>Nota:</strong> Cualquier inconveniente o consulta adicional comunicarse con el supervisor " + "<br>" +
					"<span  style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {10} " + "</span>" + "al teléfono" +
					"<span style=\" color:rgb(180, 74, 109);font-weight: 600;\">" + " {11} " + "</span>" + "o" + "escribale a" +
					"<span style=\" text-decoration: underline;color: blue;\">" + " {12} " + "</span>" + "</p>" +
			"</div>" +
			"<div style=\"padding: 10px;background: #f8f8f8;\">" +
				"<p>" + "Para asegurarse de recibir estas notificaciones, por favor agregue esta dirección de correo como remitente seguro.Gracias." +
				"</p>" +
			"</div>" +
	   "</section>" +
	"</body>" +
"</html>";
			public const string FieldService = "<!Doctype html>" +
"<html>" +
	"<head>" +
		"<meta charset=utf-8>" +
	"</head>" +

	"<body style=\"font-family: sans-serif;\">" +
		"<section style=\"with:690px\">" +
			"<div>" +
				  "<img src=" + "{12}" + ">" +
			"</div>" +
			"<div  style=\"background:rgb(255,194,15);text-align:right;\">" +
				  "<span>" + "{0}" + "</span>" +
			"</div>" +
		   " <h3>" +
				"Estimado Cliente," +
			"</h3>" +
			"<p>" +
				"En estos momentos estamos en el lugar de trabajo en" +
				"<span  style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {1} " + "</span>" + "para" + "<br>" + "iniciar la atención del servicio" + "<span  style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {2} " + "</span>." + "del equipo modelo" + "<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {3} " + "</span>" + "<br>" + "serie" + "<span  style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {4} " + "</span>" +
			"</p>" +

			"<div>" +
				"<p>" +
					"Atentamente," + "</p>" +
				"<p>" +
				"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + "{5}" + "</span>" +
				"<br>" + "<span>" + "Mecánico Líder asignado" + "</span>" + "</p>" +
			"</div>" +
			"<div>" +
				"<p>" +
				"Teléfono de contacto : " + "<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + "{6}" + "</span>" + "<br><br>" +
				"RPM de contacto : " + "<strong>" + "{7}" + "</strong>" + "<br><br>" +
				"Correo electrónico: " + "<span style=\"text-decoration:underline;color:blue;\">" + "{8}" + "</span>" + "</p>" +
			"</div>" +
			"<div style=\"padding: 10px;background: #f8f8f8;\">" +
				"<p>" +
					"<strong>" + "Nota:" + "</strong>" + "Cualquier inconveniente o consulta adicional comunicarse con el supervisor" + "<br>" + "<span  style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {9} " + "</span>" + "al teléfono" + "<span style =\" color:rgb(180, 74, 109);font-weight: 600;\">" + " {10} " + "</span>" + "o " + "escribale a" + "<br>" +
					"<span style=\"text-decoration: underline;color:blue;\">" + " {11} " + "</span>" +
				"</p>" +
			"</div>" +
	   "</section>" +
	"</body>" +
"</html>";
			public const string StandByClient = "<!Doctype html>" +
"<html>" +
	"<head>" +
		"<meta charset=utf-8>" +
	"</head>" +

	"<body style=\"font-family: sans-serif;\">" +
		"<section style=\"with:690px\">" +
			"<div>" +
				  "<img src=" + "{9}" + ">" +
			"</div>" +
			"<div  style=\"background:rgb(255,194,15);text-align:right;\">" +
				  "<span>" + "{0}" + "</span>" +
			"</div>" +
			"<h3>" + "Estimado Cliente," + "</h3>" +
			"<p>" +
				"Debido a un inconveniente ajeno a nuestra responsabilidad, nos encontramos en espera para" + "<br>" + "poder continuar con el servicio" +
				"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {1} " + "</span>" + "<br>" + "del equipo modelo" +
				"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {2} " + "</span>" + "serie" + "<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {3}. " + "</span>" + "Para mayor detalle por favor" + "<br>" + "comunicarse con el supervisor" + "<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {4} " + "</span>" + "o " + "escribale a" + "<br>" +
				"<span style=\" text-decoration: underline;color: blue;\">" + " {5} " + "</span>" +
			"</p>" +
			"<div>" +
				"<p>" +
					"Atentamente," +
				"</p>" +
				"<p>" +
					"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + "{6}" + "</span>" + "<br>" +
					"<span>" + "Mecánico Líder asignado" + "</span>" +
				"</p>" +
			"</div>" +
			"<div>" +
				"<p>" +
					"RPM de contacto : " + "<strong>" + "{7}" + "</strong>" + "<br><br>" +
					"Correo electrónico: " + "<span style=\"text-decoration: underline;color: blue;\">" + "{8}" +
					"</span>" +
				"</p>" +
			"</div>" +
			"<div style=\" padding: 10px;background: #f8f8f8;\">" +
				"<p>" +
					"Para asegurarse de recibir estas notificaciones, por favor agregue esta dirección de" + "<br>" + "correo como remitente seguro. Gracias." +
				"</p>" +
			"</div>" +
		"</section>" +
	"</body>" +
"</html>";
			public const string StandByFesa = "<!Doctype html>" +
"<html>" +
	"<head>" +
		"<meta charset=\"utf-8\">" +
		"<link rel=\"stylesheet\" href=\"css/style.css\">" +
	"</head>" +

	 "<body style=\"font-family: sans-serif;\">" +
		"<section style=\"with:690px\">" +
			"<div>" +
				  "<img src=" + "{10}" + ">" +
			"</div>" +
			"<div style=\"background:rgb(255,194,15);text-align:right;\">" +
				  "<span>{0}</span>" +
			"</div>" +
			"<h3>Estimado Cliente,</h3>" +

			"<p>Debido a un inconveniente de parte nuestra, nos encontramos en espera para poder continuar<br> con el servicio" +
			"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">{1}</span> del equipo modelo <span style=\"color:rgb(180, 74, 109);font-weight: 600;\"> {2} </span> serie <br><span style=\"color:rgb(180, 74, 109);font-weight: 600;\"> {3} </span>. Para mayor detalle por favor comunicarse con el supervisor <span style=\"color:rgb(180, 74, 109);font-weight: 600;\">{4} </span><br> al teléfono <span class=\"color:rgb(180, 74, 109);font-weight: 600;\"> {5} </span> o escribale a <span style=\"text-decoration: underline;color: blue;\"> {6} </span></p>" +
			"<div>" +
				"<p>Atentamente,</p>" +
				"<p><span  style=\"color:rgb(180, 74, 109);font-weight: 600;\">{7}</span><br><span>Mecánico Líder asignado</span></p>" +
			"</div>" +
			"<div>" +
				"<p>RPM de contacto : <strong>{8}</strong><br>" +
				"Correo electrónico: <span style=\"text-decoration: underline;color: blue;\">{9}</span></p>" +
			"</div>" +
			"<div style=\"padding: 10px;background: #f8f8f8;\">" +
				"<p>Para asegurarse de recibir estas notificaciones, por favor agregue esta dirección de <br> correo como remitente seguro. Gracias.</p>" +
			"</div>" +
		"</section>" +

	"</body>" +

"</html>";

			public const string FieldReport = "<!Doctype html>" +
"<html>" +
	"<head>" +
		"<meta charset=utf-8>" +
	"</head>" +
	"<body style=\"font-family: sans-serif;\">" +
		"<section style=\"with:690px>" +
			"<div>" +
				  "<img src=" + "{10}" + ">" +
			"</div>" +
			"<div style=\"background:rgb(255,194,15);text-align:right;\">" +
				  "<span> {0} </span>" +
			"</div>" +
			"<h3>" +
				"Estimado Cliente," +
			"</h3>" +
			"<p>" +
				 "Hemos finalizado con el servicio " +
				"<span style=\" color:rgb(180, 74, 109);font-weight: 600;\">" +
					"{1}" + "</span>" + "<br>" + "del equipo modelo" +
				"<span style=\" color:rgb(180, 74, 109);font-weight: 600;\">" + " {2} " + "</span>" +
				" serie " +
				"<span style=\" color:rgb(180, 74, 109);font-weight: 600;\">" + " {3} " + "</span>" + "ubicado en" +
				"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {4} " + "</span>." +
			"</p>" +
			"<p>" +
				 "Estamos procediendo a retirarnos." +
			"</p>" +
			"<p>" +
				"Muy pronto le estaremos enviando el informe del servicio realizado." +
			"</p>" +
			"<div>" +
				"<p>" +
					"Atentamente," +
				"</p>" +
				"<p>" +
					"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + "{5}" +
					"</span>" + "<br>" + "<span>" + "Mecánico Líder Asignado" + "</span>" +
				"</p>" +
			"</div>" +
			"<div>" +
				"<p>" +
					"RPM de contacto :" + "<strong>" + "{6}" + "</strong>" + "<br>" +
					"Correo electrónico: " + "<span style=\"text-decoration: underline;color:blue;\">" + "{7}" + "</span>" +
				"</p>" +
			"</div>" +
			"<div  style=\" padding: 10px;background: #f8f8f8;\">" +
				"<p>" +
					"<strong>" + "Nota:" + "</strong>" + "Cualquier inconveniente o consulta adicional comunicarse con el supervisor" + "<br>" +
					"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + " {8} " + "</span>" + "o" + "escribale a" +
					"<span style=\"text-decoration: underline;color: blue;\">" + " {9}" +
					"</span>" +
				"</p>" +
			"</div>" +

			"<div style=\" padding: 10px;background: #f8f8f8;\">" +
				"<p>" + "Para asegurarse de recibir estas notificaciones, por favor agregue esta dirección de" + "<br>" + "correo como remitente seguro. Gracias." + "</p>" +
			"</div>" +
		"</section>" +
	"</body>" +
"</html>";

			public const string TravelEnd = "<!Doctype html>" +
"<html>" +
	"<head>" +
		"<meta charset=utf-8>  " +
	"</head>" +

	"<body style=\"font-family: sans-serif;\">" +
		"<section style=\"with:690px;\">" +
			"<div>" +
				  "<img src=" + "{7}" + ">" +
			"</div>" +
			"<div style=\"background:rgb(255,194,15);text-align:right;\">" +
				"<span>" + "{0}" + "</span>" +
			"</div>" +
			"<h3>" +
				"Estimado Cliente," +
			"</h3>" +
			"<p>" +
				"El técnico" +
				"<span style=\"  color:rgb(180, 74, 109);font-weight: 600;\">"
				+ "{1}" +
				"</span>" + "ha finalizado el viaje de retorno de la OT " +
				  "<br>" +
				"<span style=\" color:rgb(180, 74, 109);font-weight: 600;\">" + "{2}" + "</span>" + "del servicio" + "<span style=\" color:rgb(180, 74, 109);font-weight: 600;\">" + "{3}" + "</span>," + "cliente" + "<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + "{4}" +
				"</span>," + "modelo" + "<br>" + "<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + "{5}" + "</span>," + "serie" +
				"<span style=\"color:rgb(180, 74, 109);font-weight: 600;\">" + "{5}" + "</span>" +
			"</p>" +

			"<div>" +
				"<p>" + "Atentamente," + "</p>" +
				"<p>" + "<span style=\" color:rgb(180, 74, 109);font-weight: 600;\">" + "{6}" + "</span>" +
				"<br>" +
				"</p>" +
			"</div>" +
			"<div style=\"padding: 10px;background: #f8f8f8;\">" +
				"<p>" +
					"Para asegurarse de recibir estas notificaciones, por favor agregue esta dirección de" + "<br>" + "correo como remitente seguro. Gracias." +
				"</p>" +
			"</div>" +
		"</section>" +
	"</body>" +
"</html>";
		}

		public static class Mailsubject {
			public const string Traveling = "El servicio {0} cambió de estado a: Viajando";
			public const string Driving = "El servicio {0} cambió de estado a: Manejando";
			public const string FieldService = "El servicio {0} cambió de estado a: Servicio en campo";
			public const string StandByClient = "El servicio {0} cambió de estado a: Espera por cliente";
			public const string StandByFesa = "El servicio {0} cambió de estado a: Espera por Ferreyros";
			public const string FieldReport = "El servicio {0} cambió de estado a: Finalizado";
			public const string TravelEnd = "El servicio {0} cambió de estado a: Finalizado";
		}

		public class MessageBodyActivities {

			//PRE
			public const string PreparingTripTemplateStart = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha iniciado su preparación para viajar a [Dirección del cliente] y atender al cliente [Nombre del cliente].";
			public const string PreparingTripStart = "Hola {0}, {1} ha iniciado su preparación para viajar a {2} y atender al cliente {3}.";

			public const string PreparingTripTemplateEnd = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha finalizado su preparación para viajar a [Dirección del cliente] y atender al cliente [Nombre del cliente].";
			public const string PreparingTripEnd = "Hola {0}, {1} ha finalizado su preparación para viajar a {2} y atender al cliente {3}.";

			public const string TravelingSupervisorTemplateStart = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha iniciado su viaje a [Dirección del cliente] para atender al cliente [Nombre del cliente].";
			public const string TravelingSupervisorStart = "Hola {0}, {1} ha iniciado su viaje a {2} para atender al cliente {3}.";

			public const string TravelingSupervisorTemplateEnd = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha finalizado su viaje a [Dirección del cliente] para atender al cliente [Nombre del cliente].";
			public const string TravelingSupervisorEnd = "Hola {0}, {1} ha finalizado su viaje a {2} para atender al cliente {3}.";

			public const string TravelingClienteTemplateStart = "Estimado [Nombre del Contacto] Hemos iniciado nuestro viaje a sus instalaciones ubicadas en [Dirección del cliente] para atender el servicio solicitado.";
			public const string TravelingClienteStart = "Estimado {0} Hemos iniciado nuestro viaje a sus instalaciones ubicadas en {1} para atender el servicio solicitado.";

			public const string DrivingSupervisorTemplateStart = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha iniciado la ruta a [Dirección del cliente] para atender al cliente [Nombre del cliente].";
			public const string DrivingSupervisorStart = "Hola {0}, {1} ha iniciado la ruta a {2} para atender al cliente {3}.";

			public const string DrivingSupervisorTemplateEnd = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha finalizado la ruta a [Dirección del cliente] para atender al cliente [Nombre del cliente].";
			public const string DrivingSupervisorEnd = "Hola {0}, {1} ha finalizado la ruta a {2} para atender al cliente {3}.";

			public const string DrivingClienteTemplateStart = "Estimado [Nombre del Contacto] Hemos iniciado nuestro trayecto a sus instalaciones ubicadas en [Dirección del cliente] para atender el servicio solicitado.";
			public const string DrivingClienteStart = "Estimado {0} Hemos iniciado nuestro trayecto a sus instalaciones ubicadas en {1} para atender el servicio solicitado.";

			public const string DelaySupervisorTemplateStart = "Hola [Nombre del Supervisor], [Nombre del Técnico] se encuentra en estado de espera para iniciar atención al servicio.";
			public const string DelaySupervisorStart = "Hola {0}, {1} se encuentra en estado de espera para iniciar atención al servicio.";

			public const string DelaySupervisorTemplateEnd = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha finalizado la espera.";
			public const string DelaySupervisorEnd = "Hola {0}, {1} ha finalizado la espera.";

			//DURANTE
			public const string FieldServiceSupervisorTemplateStart = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha iniciado el servicio de campo.";
			public const string FieldServiceSupervisorStart = "Hola {0}, {1} ha iniciado el servicio de campo.";

			public const string FieldServiceClienteTemplateStart = "Estimado [Nombre del Contacto], hemos iniciado las actividades del servicio.";
			public const string FieldServiceClienteStart = "Estimado {0}, hemos iniciado las actividades del servicio.";

			public const string StandByClientSupervisorTemplateStat = "Hola [Nombre del Supervisor], [Nombre del Técnico] ya llegó a [Dirección] y está a la espera de iniciar el servicio de la OT [Nro de OT] por una demora ocasionada por el cliente.";
			public const string StandByClientSupervisorStart = "Hola {0}, {1} ya llegó a {2} y está a la espera de iniciar el servicio de la OT {3} por una demora ocasionada por el cliente.";

			public const string StandByClientSupervisorTemplateEnd = "Hola [Nombre del Supervisor], la espera ocasionada por el cliente terminó.";
			public const string StandByClientSupervisorEnd = "Hola {0}, la espera ocasionada por el cliente terminó.";

			public const string StandByClientClienteTemplateStart = "Estimado [Nombre del Contacto], debido a un inconveniente ajeno a nuestra responsabilidad, nos encontramos a la espera para continuar con el servicio. Por favor contactar al personal asignado por cualquier consulta que pueda tener. Estaremos atentos a retomar acción cuando sea posible.";
			public const string StandByClientClienteStart = "Estimado {0}, debido a un inconveniente ajeno a nuestra responsabilidad, nos encontramos a la espera para continuar con el servicio. Por favor contactar al personal asignado por cualquier consulta que pueda tener. Estaremos atentos a retomar acción cuando sea posible.";

			public const string FieldReportSupervisorTemplateEnd = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha terminado el servicio de campo. Por favor aprobar a la brevedad el informe preliminar de servicio, desde la aplicación web, para que sea enviado al cliente por correo electrónico.";
			public const string FieldReportSupervisorEnd = "Hola {0}, {1} ha terminado el servicio de campo. Por favor aprobar a la brevedad el informe preliminar de servicio, desde la aplicación web, para que sea enviado al cliente por correo electrónico.";

			public const string FieldReportClienteTemplateEnd = "Estimado [Nombre del Contacto] Hemos terminado con el servicio solicitado. En unas horas recibirá, vía correo electrónico, el informe preliminar de servicios y luego de unos días, el informe final por el servicio realizado. </br> Si tiene alguna consulta adicional, por favor contactar al personal indicado en este correo. Esperamos que nuestra atención haya sido la esperada. Gracias por confiar en Ferreyros.";
			public const string FieldReportClienteEnd = "Estimado {0} Hemos terminado con el servicio solicitado. En unas horas recibirá, vía correo electrónico, el informe preliminar de servicios y luego de unos días, el informe final por el servicio realizado. </br> Si tiene alguna consulta adicional, por favor contactar al personal indicado en este correo. Esperamos que nuestra atención haya sido la esperada. Gracias por confiar en Ferreyros.";

			public const string StandByFesaSupervisorTemplateStart = "Hola [Nombre del Supervisor], [Nombre del Técnico] ya llegó a [Dirección] y está a la espera de solucionar un inconveniente para dar inicio al servicio de la OT [Nro de OT].";
			public const string StandByFesaSupervisorStart = "Hola {0}, {1} ya llegó a {2} y está a la espera de solucionar un inconveniente para dar inicio al servicio de la OT {3}.";

			//POST
			public const string InformeFinalSupervisorTemplateEnd = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha subido el informe final del servicio a la plataforma web. Por favor aprobarlo a la brevedad posible para que sea enviado al cliente por correo electrónico.";
			public const string InformeFinalSupervisorEnd = "Hola {0}, {1} ha subido el informe final del servicio a la plataforma web. Por favor aprobarlo a la brevedad posible para que sea enviado al cliente por correo electrónico.";

			public const string DrivingReturnSupervisorTemplateStart = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha iniciado la ruta de regreso.";
			public const string DrivingReturnSupervisorStart = "Hola {0}, {1} ha iniciado la ruta de regreso.";

			public const string DrivingReturnSupervisorTemplateEnd = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha finalizado la ruta de regreso.";
			public const string DrivingReturnSupervisorEnd = "Hola {0}, {1} ha finalizado la ruta de regreso.";

			public const string TravelingReturnTemplateStart = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha iniciado su viaje de regreso.";
			public const string TravelingReturnStart = "Hola {0}, {1} ha iniciado su viaje de regreso.";

			public const string TravelingReturnTemplateEnd = "Hola [Nombre del Supervisor], [Nombre del Técnico] ha finalizado su viaje a [Dirección del cliente] para atender al cliente [Nombre del cliente].";
			public const string TravelingReturnEnd = "Hola {0}, {1} ha finalizado su viaje a {2} para atender al cliente {3}.";

			//OFF
			public const string ActivitiesClienteTemplateOffile = "Estimado [Nombre de cliente],Las siguientes actividades no se notificaron a tiempo por baja señal celular en la zona. Estos fueron los tiempos registrados para el servicio de la OT[Nro de OT]:";
			public const string ActivitiesOffile = "Estimado {0}, las siguientes actividades no se notificaron a tiempo por baja señal celular en la zona. Estos fueron los tiempos registrados para el servicio de la OT {1}:" + "</br>" + " <ul> {2} </ul>";

        }



        public class Message {

			public const string Prueba = "Hola Mundo";

		}

		public class AzureStorage {

			public const string DefaultConnectionString = "DefaultEndpointsProtocol=https;AccountName=serviciodecampo;AccountKey=Ev0LRkUoub7rzwDGR8N1xF6dgALm+O8Pzv767eXNUfXYRtyNXuFX5HilPj9+fpcMUxMCVSkfXGo484Y7xvvTmA==;EndpointSuffix=core.windows.net";

		}


	}
}