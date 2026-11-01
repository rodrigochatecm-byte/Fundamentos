using System;
using System.IO;

namespace Parte_5_Examen_Parcial
{
    internal class Program
    { 
        static int max_documentos = 500;
        static int total_documentos = 0;
        static string[] codigos = new string[500];
        static string[] fechas = new string[500];
        static string[] tipos = new string[500];
        static string[] descripciones = new string[500];
        static string[] nombres_usuarios = new string[500];
        static string[] dni_usuarios = new string[500];
        static string[] telefonos_usuarios = new string[500];
        static string[] nombres_oficiales = new string[500];
        static string[] correos_oficiales = new string[500];
        static string[] telefonos_oficiales = new string[500];
        static string[] fechas_ingreso_oficial = new string[500];
        static string nombre_oficial = "";
        static string correo_oficial = "";
        static string telefono_oficial = "";
        static string fecha_ingreso = "";

        static string carpeta = "DatosMesaPartes";

        static void Main(string[] args)
        {
            Console.WriteLine("MESA DE PARTES");
            string contrasena = "";
            int intentos = 0;

            while (contrasena != "PNP2026" && intentos < 3)
            {
                Console.Write("Ingrese la contraseña: ");
                contrasena = Console.ReadLine();
                intentos++;

                if (contrasena != "PNP2026")
                {
                    Console.WriteLine("Contraseña incorrecta.");
                }
            }

            if (contrasena != "PNP2026")
            {
                Console.WriteLine("Se acabaron los intentos.");
                return;
            }
            Directory.CreateDirectory(carpeta);
            Directory.CreateDirectory(carpeta + "/SOLICITUD");
            Directory.CreateDirectory(carpeta + "/OFICIO");
            Directory.CreateDirectory(carpeta + "/CARTA");
            CargarDocumentos();
            Console.WriteLine("\nDatos del oficial encargado");
            Console.Write("Nombre: ");
            nombre_oficial = Console.ReadLine();

            Console.Write("Correo electrónico: ");
            correo_oficial = Console.ReadLine();

            telefono_oficial = LeerTelefono("Teléfono (9 dígitos): ");
            fecha_ingreso = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            int opcion = 0;

            while (opcion != 9)
            {
                Console.WriteLine("\n--- MESA DE PARTES ---");
                Console.WriteLine("1. Registrar documento");
                Console.WriteLine("2. Buscar documento");
                Console.WriteLine("3. Ver documentos de un DNI");
                Console.WriteLine("4. Eliminar documento");
                Console.WriteLine("5. Modificar documento o usuario");
                Console.WriteLine("6. Modificar datos del oficial");
                Console.WriteLine("7. Ver seguimiento");
                Console.WriteLine("8. Ayuda");
                Console.WriteLine("9. Salir");
                Console.WriteLine("10. Clean (limpiar pantalla)");

                opcion = LeerNumero("Elija una opción: ", 1, 10);

                switch (opcion)
                {
                    case 1:
                        RegistrarDocumento();
                        break;
                    case 2:
                        BuscarDocumento();
                        break;
                    case 3:
                        VerDocumentosDni();
                        break;
                    case 4:
                        EliminarDocumento();
                        break;
                    case 5:
                        ModificarDocumento();
                        break;
                    case 6:
                        ModificarOficial();
                        break;
                    case 7:
                        VerSeguimiento();
                        break;
                    case 8:
                        MostrarAyuda();
                        break;
                    case 9:
                        GuardarSeguimiento();
                        Console.WriteLine("Programa cerrado.");
                        break;
                    case 10:
                        Console.Clear();
                        break;
                }
            }
        }

        static void RegistrarDocumento()
        {
            if (total_documentos >= max_documentos)
            {
                Console.WriteLine("Se alcanzó el máximo de documentos.");
                return;
            }

            Console.WriteLine("Tipo de documento:");
            Console.WriteLine("1. Solicitud");
            Console.WriteLine("2. Oficio");
            Console.WriteLine("3. Carta");
            int opcion_tipo = LeerNumero("Elija el tipo: ", 1, 3);

            if (opcion_tipo == 1)
            {
                tipos[total_documentos] = "SOLICITUD";
            }
            else if (opcion_tipo == 2)
            {
                tipos[total_documentos] = "OFICIO";
            }
            else
            {
                tipos[total_documentos] = "CARTA";
            }

            fechas[total_documentos] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            codigos[total_documentos] = "DOC-" + DateTime.Now.ToString("yyyyMMddHHmmss") + total_documentos;

            Console.Write("Nombre del usuario: ");
            nombres_usuarios[total_documentos] = Console.ReadLine();

            dni_usuarios[total_documentos] = LeerDni("DNI (8 dígitos): ");
            telefonos_usuarios[total_documentos] = LeerTelefono("Teléfono del usuario (9 dígitos): ");

            Console.Write("Descripción del documento: ");
            descripciones[total_documentos] = Console.ReadLine();

            nombres_oficiales[total_documentos] = nombre_oficial;
            correos_oficiales[total_documentos] = correo_oficial;
            telefonos_oficiales[total_documentos] = telefono_oficial;
            fechas_ingreso_oficial[total_documentos] = fecha_ingreso;

            Console.Write("¿Desea guardar el documento? (S/N): ");
            string respuesta = Console.ReadLine().ToUpper();

            if (respuesta == "S")
            {
                total_documentos++;
                GuardarDocumentos();
                Console.WriteLine("Documento guardado.");
                Console.WriteLine("Código: " + codigos[total_documentos - 1]);
            }
            else
            {
                Console.WriteLine("El documento no se guardó.");
            }
        }

        static void BuscarDocumento()
        {
            Console.Write("Ingrese código, DNI o tipo de documento: ");
            string busqueda = Console.ReadLine().ToUpper();
            int encontrados = 0;
            for (int i = 0; i < total_documentos; i++)
            {
                if (codigos[i].ToUpper() == busqueda ||
                    dni_usuarios[i] == busqueda ||
                    tipos[i] == busqueda)
                {
                    MostrarDocumento(i);
                    encontrados++;
                }
            }

            if (encontrados == 0)
            {
                Console.WriteLine("No se encontraron documentos.");
            }
        }

        static void VerDocumentosDni()
        {
            string dni = LeerDni("Ingrese el DNI: ");
            int cantidad = 0;

            for (int i = 0; i < total_documentos; i++)
            {
                if (dni_usuarios[i] == dni)
                {
                    MostrarDocumento(i);
                    cantidad++;
                }
            }

            Console.WriteLine("Esta persona tiene " + cantidad + " documento(s).");
        }

        static void EliminarDocumento()
        {
            string dni = LeerDni("Ingrese el DNI del usuario: ");
            int cantidad = 0;

            for (int i = 0; i < total_documentos; i++)
            {
                if (dni_usuarios[i] == dni)
                {
                    MostrarDocumento(i);
                    cantidad++;
                }
            }

            if (cantidad == 0)
            {
                Console.WriteLine("No hay documentos de ese DNI.");
                return;
            }

            Console.Write("Escriba el código del documento que desea eliminar: ");
            string codigo = Console.ReadLine();
            int posicion = -1;

            for (int i = 0; i < total_documentos; i++)
            {
                if (dni_usuarios[i] == dni && codigos[i] == codigo)
                {
                    posicion = i;
                }
            }

            if (posicion == -1)
            {
                Console.WriteLine("No se encontró ese código para el DNI indicado.");
                return;
            }

            for (int i = posicion; i < total_documentos - 1; i++)
            {
                CopiarDocumento(i + 1, i);
            }

            total_documentos--;
            GuardarDocumentos();
            Console.WriteLine("Documento eliminado.");
        }

        static void ModificarDocumento()
        {
            Console.Write("Ingrese el código del documento: ");
            string codigo = Console.ReadLine();
            int posicion = BuscarPorCodigo(codigo);

            if (posicion == -1)
            {
                Console.WriteLine("No se encontró el documento.");
                return;
            }

            Console.WriteLine("Deje vacío un campo si desea conservar el dato actual.");

            Console.Write("Nombre actual: " + nombres_usuarios[posicion] + ". Nuevo nombre: ");
            string nombre = Console.ReadLine();
            if (nombre != "")
            {
                nombres_usuarios[posicion] = nombre;
            }

            Console.Write("DNI actual: " + dni_usuarios[posicion] + ". Nuevo DNI: ");
            string dni = Console.ReadLine();
            if (dni != "")
            {
                if (EsNumeroDeLongitud(dni, 8))
                {
                    dni_usuarios[posicion] = dni;
                }
                else
                {
                    Console.WriteLine("El DNI debe tener 8 dígitos. Se mantiene el DNI anterior.");
                }
            }

            Console.Write("Teléfono actual: " + telefonos_usuarios[posicion] + ". Nuevo teléfono: ");
            string telefono = Console.ReadLine();
            if (telefono != "")
            {
                if (EsNumeroDeLongitud(telefono, 9))
                {
                    telefonos_usuarios[posicion] = telefono;
                }
                else
                {
                    Console.WriteLine("El teléfono debe tener 9 dígitos. Se mantiene el teléfono anterior.");
                }
            }

            Console.Write("Descripción actual: " + descripciones[posicion] + ". Nueva descripción: ");
            string descripcion = Console.ReadLine();
            if (descripcion != "")
            {
                descripciones[posicion] = descripcion;
            }

            GuardarDocumentos();
            Console.WriteLine("Documento modificado.");
        }

        static void ModificarOficial()
        {
            Console.Write("Nuevo nombre del oficial: ");
            nombre_oficial = Console.ReadLine();

            Console.Write("Nuevo correo electrónico: ");
            correo_oficial = Console.ReadLine();

            telefono_oficial = LeerTelefono("Nuevo teléfono (9 dígitos): ");
            fecha_ingreso = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            Console.WriteLine("Los nuevos datos se usarán en los documentos que registre desde ahora.");
        }

        static void VerSeguimiento()
        {
            Console.WriteLine("\n--- DOCUMENTOS REGISTRADOS ---");

            for (int mes = 1; mes <= 12; mes++)
            {
                for (int i = 0; i < total_documentos; i++)
                {
                    DateTime fecha_documento = DateTime.ParseExact(fechas[i], "dd/MM/yyyy HH:mm:ss", null);

                    if ((int)fecha_documento.Month == mes)
                    {
                        MostrarDocumento(i);
                    }
                }
            }

            Console.WriteLine("\nEl seguimiento completo está en: " + carpeta + "/encargados_y_seguimiento.txt");
        }

        static void MostrarAyuda()
        {
            Console.WriteLine("\nAYUDA DE LAS OPCIONES");
            Console.WriteLine("1. Registrar un documento nuevo.");
            Console.WriteLine("2. Buscar documentos por código, DNI o tipo.");
            Console.WriteLine("3. Contar los documentos registrados para un DNI.");
            Console.WriteLine("4. Eliminar un documento eligiéndolo por su código y DNI.");
            Console.WriteLine("5. Cambiar datos del usuario o la descripción del documento.");
            Console.WriteLine("6. Cambiar los datos del oficial para próximos registros.");
            Console.WriteLine("7. Ver el seguimiento de documentos ordenado por mes.");
            Console.WriteLine("8. Mostrar esta ayuda.");
            Console.WriteLine("9. Guardar el seguimiento y salir.");
        }

        static void MostrarDocumento(int posicion)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Código: " + codigos[posicion]);
            Console.WriteLine("Fecha: " + fechas[posicion]);
            Console.WriteLine("Tipo: " + tipos[posicion]);
            Console.WriteLine("Usuario: " + nombres_usuarios[posicion]);
            Console.WriteLine("DNI: " + dni_usuarios[posicion]);
            Console.WriteLine("Teléfono usuario: " + telefonos_usuarios[posicion]);
            Console.WriteLine("Descripción: " + descripciones[posicion]);
            Console.WriteLine("Oficial: " + nombres_oficiales[posicion]);
            Console.WriteLine("Correo oficial: " + correos_oficiales[posicion]);
            Console.WriteLine("Teléfono oficial: " + telefonos_oficiales[posicion]);
        }

        static int BuscarPorCodigo(string codigo)
        {
            for (int i = 0; i < total_documentos; i++)
            {
                if (codigos[i] == codigo)
                {
                    return i;
                }
            }

            return -1;
        }

        static void CopiarDocumento(int origen, int destino)
        {
            codigos[destino] = codigos[origen];
            fechas[destino] = fechas[origen];
            tipos[destino] = tipos[origen];
            descripciones[destino] = descripciones[origen];
            nombres_usuarios[destino] = nombres_usuarios[origen];
            dni_usuarios[destino] = dni_usuarios[origen];
            telefonos_usuarios[destino] = telefonos_usuarios[origen];
            nombres_oficiales[destino] = nombres_oficiales[origen];
            correos_oficiales[destino] = correos_oficiales[origen];
            telefonos_oficiales[destino] = telefonos_oficiales[origen];
            fechas_ingreso_oficial[destino] = fechas_ingreso_oficial[origen];
        }

        static void GuardarDocumentos()
        {
            GuardarTipo("SOLICITUD");
            GuardarTipo("OFICIO");
            GuardarTipo("CARTA");
        }

        static void GuardarTipo(string tipo)
        {
            string ruta = carpeta + "/" + tipo + "/documento.txt";

            using (StreamWriter archivo = new StreamWriter(ruta))
            {
                for (int i = 0; i < total_documentos; i++)
                {
                    if (tipos[i] == tipo)
                    {
                        string linea = codigos[i] + "|" + fechas[i] + "|" + tipos[i] + "|" +
                            descripciones[i] + "|" + nombres_usuarios[i] + "|" + dni_usuarios[i] + "|" +
                            telefonos_usuarios[i] + "|" + nombres_oficiales[i] + "|" + correos_oficiales[i] + "|" +
                            telefonos_oficiales[i] + "|" + fechas_ingreso_oficial[i];
                        archivo.WriteLine(linea);
                    }
                }
            }
        }

        static void CargarDocumentos()
        {
            CargarTipo("SOLICITUD");
            CargarTipo("OFICIO");
            CargarTipo("CARTA");
        }

        static void CargarTipo(string tipo)
        {
            string ruta = carpeta + "/" + tipo + "/documento.txt";

            if (!File.Exists(ruta))
            {
                return;
            }

            using (StreamReader archivo = new StreamReader(ruta))
            {
                string linea;

                while ((linea = archivo.ReadLine()) != null && total_documentos < max_documentos)
                {
                    string[] partes = linea.Split('|');

                    if (partes.Length == 11)
                    {
                        codigos[total_documentos] = partes[0];
                        fechas[total_documentos] = partes[1];
                        tipos[total_documentos] = partes[2];
                        descripciones[total_documentos] = partes[3];
                        nombres_usuarios[total_documentos] = partes[4];
                        dni_usuarios[total_documentos] = partes[5];
                        telefonos_usuarios[total_documentos] = partes[6];
                        nombres_oficiales[total_documentos] = partes[7];
                        correos_oficiales[total_documentos] = partes[8];
                        telefonos_oficiales[total_documentos] = partes[9];
                        fechas_ingreso_oficial[total_documentos] = partes[10];
                        total_documentos++;
                    }
                }
            }
        }

        static void GuardarSeguimiento()
        {
            string ruta = carpeta + "/encargados_y_seguimiento.txt";

            using (StreamWriter archivo = new StreamWriter(ruta, true))
            {
                archivo.WriteLine("----------------------------------------");
                archivo.WriteLine("Oficial: " + nombre_oficial);
                archivo.WriteLine("Correo: " + correo_oficial);
                archivo.WriteLine("Teléfono: " + telefono_oficial);
                archivo.WriteLine("Fecha de ingreso: " + fecha_ingreso);
                archivo.WriteLine("Fecha de salida: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                for (int i = 0; i < total_documentos; i++)
                {
                    if (nombres_oficiales[i] == nombre_oficial && fechas_ingreso_oficial[i] == fecha_ingreso)
                    {
                        archivo.WriteLine(codigos[i] + " - " + tipos[i] + " - DNI: " + dni_usuarios[i] + " - " + nombres_usuarios[i]);
                    }
                }
            }
        }

        static string LeerDni(string mensaje)
        {
            string dni = "";

            while (!EsNumeroDeLongitud(dni, 8))
            {
                Console.Write(mensaje);
                dni = Console.ReadLine();

                if (!EsNumeroDeLongitud(dni, 8))
                {
                    Console.WriteLine("El DNI debe tener exactamente 8 dígitos.");
                }
            }

            return dni;
        }

        static string LeerTelefono(string mensaje)
        {
            string telefono = "";

            while (!EsNumeroDeLongitud(telefono, 9))
            {
                Console.Write(mensaje);
                telefono = Console.ReadLine();

                if (!EsNumeroDeLongitud(telefono, 9))
                {
                    Console.WriteLine("El teléfono debe tener exactamente 9 dígitos.");
                }
            }

            return telefono;
        }

        static bool EsNumeroDeLongitud(string texto, int cantidad)
        {
            if (texto == null || texto.Length != cantidad)
            {
                return false;
            }

            for (int i = 0; i < texto.Length; i++)
            {
                if (texto[i] < '0' || texto[i] > '9')
                {
                    return false;
                }
            }

            return true;
        }

        static int LeerNumero(string mensaje, int minimo, int maximo)
        {
            int numero = 0;
            bool correcto = false;

            while (!correcto)
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine();

                if (int.TryParse(entrada, out numero) && numero >= minimo && numero <= maximo)
                {
                    correcto = true;
                }
                else
                {
                    Console.WriteLine("Ingrese un número del " + minimo + " al " + maximo + ".");
                }
            }

            return numero;
        }
    }
}