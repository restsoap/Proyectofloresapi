﻿using Proyectofloresapi.Filters;
using Proyectofloresapi.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Proyectofloresapi.Controllers
{
    public class AnalisisController : Controller
    {
        // GET: Analisis

        [AuthorizeUser(idOperacion: 26)]
        public ActionResult ListaAnalisis()
        {
            using (proyectofloresEntities db = new proyectofloresEntities())
            {

                var oBloque = new bloque();

                var query = (from c in db.bloque
                             select c.diferenciaaño).ToArray();

                //hallar el minimo valor
                var min = query.Min();
                //hallar el maximo valor
                var max = query.Max();

                int bloquecount = db.bloque.Count();
                Console.WriteLine(bloquecount);

                //cargar los datos de la base al array 
                double[] vector = query;

                //numero de datos que vienen de la base de datos
                int columnas = bloquecount;

                //calculamos el numero de intervalos
                double intervalos = Math.Log(columnas + 1, 2);

                //redondeamos el numero de intervalos
                int redondeo = (int)Math.Round(intervalos);

                //hallar el rango 
                int rango = (int)(max - min);
                Console.WriteLine(rango);

                //amplitud de intervalos
                double amp_Rango = rango / redondeo;
                Console.WriteLine(amp_Rango);

                double[] linf = new double [redondeo];
                double[] lsup = new double [redondeo];
                double[] mclase = new double[redondeo];
                double[] fabsoluta = new double[redondeo];
                double[] frelativa = new double[redondeo];
                double[] xf = new double[redondeo];
                int i = 0, j = 0; 
                for (i = 0; i<redondeo; i++)
                {
                    if (i ==0) {
                        linf[i] = min;
                        ViewBag.linf = linf;
                    }   

                    for(j = 0; j < redondeo; j++)
                    {
                        if (i == 0)
                        {
                            lsup[j] = linf[i] + amp_Rango;
                        }

                        if(i > 0 && j > 0)
                        {
                            linf[i] = lsup[j - 1] + 0.1;
                            
                        }

                        if (j > 0)
                        {
                            lsup[j] = linf[i] + amp_Rango;
                        }

                        ViewBag.lsup = lsup;
                        
                    }
                }

                //amplitud
                int a = 0;
                i = 0 ; j = 0;
                double[] amplitud = new double[redondeo];
                for(a = 0; a <redondeo; a++)
                {
                    amplitud[a] = lsup[j] - linf[i];
                    i++;
                    j++;
                }

                //marca de clase
                int l = 0, m = 0;
                for (int k = 0; k < redondeo; k++)
                {                 
                    mclase[k] = ((linf[l] + lsup[m]) / 2);
                    l++;
                    m++;
                }
                ViewBag.marca_clase = mclase;

                //frecuencia absoluta
                l = 0; m = 0; 
                double acumulado = 0;
                int o, r=0;
                double[] acum_fabsol = new double[redondeo]; 
                for (int n = 0; n < redondeo; n++)
                {
                    for (o = 0; o < columnas; o++)
                    {
                        if (vector[o] >= linf[l] && vector[o] <= lsup[m])
                        {
                            fabsoluta[n] += 1;
                        }
                    }
                    l++;
                    m++;

                    acumulado += fabsoluta[n];
                    if(r ==0  && n == 0)
                    {
                        acum_fabsol[r] = fabsoluta[n];
                    }
                    if (r > 0 && n > 0)
                    {
                        acum_fabsol[r] = fabsoluta[n]+ fabsoluta[n-1];
                    }
                    r++;

                }
                ViewBag.fabsoluta = fabsoluta;

                //frecuencia relativa 
                o = 0;
                for (int p = 0; p<redondeo; p++)
                {
                    frelativa[p] = fabsoluta[o] / acumulado;
                    o++;
                }
                ViewBag.frelativa = frelativa;

                //xf
                int mc = 0, fa = 0;
                double acumxf = 0;
                for (int q = 0; q<redondeo; q++)
                {
                    xf[q] = mclase[mc] * fabsoluta[fa];
                    acumxf += xf[q];
                    mc++;
                    fa++;
                }
                ViewBag.xf = xf;

                //media
                double media = acumxf / acumulado;
                ViewBag.media = media;

                //mediana
                int acum = 0, dato = 0, Fi_i = 0;
                double li = 0, amp = 0, mediana = 0 ;
                r = 0; i = 0; a = 0 ;
                if (acumulado % 2 == 0)
                {
                    //acumulado/2
                    acum = (int) Math.Round(acumulado / 2);
                    for (int s = 0; s < redondeo; s++)
                    {

                        if (acum < acum_fabsol[s] && s == 0)
                        {
                            Fi_i = 0;
                            dato = (int)acum_fabsol[s];
                            //valor del limite inferior
                            if (i == s && a == s)
                            {
                                li = linf[i];
                                amp = amplitud[a];
                            }
                        }

                        if (s > 0)
                        {
                            if (acum > acum_fabsol[s-1] && acum < acum_fabsol[s])
                            {
                                Fi_i = (int)acum_fabsol[s - 1];
                                dato = (int)acum_fabsol[s];
                                //valor del limite inferior
                                if (i == s && a == s)
                                {
                                    li = linf[i];
                                    amp = amplitud[a];
                                }
                            }
                        }

                        i++;
                        a++;
                    }

                    mediana = li + ((acumulado / 2) - Fi_i / dato) * amp;
                }
                else
                {
                    acum = (int)Math.Round((acumulado + 1) / 2);
                    for (int s = 0; s < redondeo; s++)
                    {
                        if (acum < acum_fabsol[s] && s == 0)
                        {
                            Fi_i = 0;
                            dato = (int)acum_fabsol[s];
                            //valor del limite inferior
                            if (i == s && a == s)
                            {
                                li = linf[i];
                                amp = amplitud[a];
                            }
                        }
                        if (s > 0)
                        {           
                            if (acum > acum_fabsol[s - 1] && acum < acum_fabsol[s])
                            {
                                Fi_i = (int)acum_fabsol[s - 1];
                                dato = (int)acum_fabsol[s];
                                //valor del limite inferior
                                if (i == s && a == s)
                                {
                                    li = linf[i];
                                    amp = amplitud[a];
                                }
                            }
                        }
                        i++;
                        a++;
                    }

                    //mediana = li + ((acumulado / 2) - Fi_i / dato) * amp;
                }
                //imprimos la mediana
                //ViewBag.mediana = mediana;




                //analisis de produccion 

                var oProduccion = new produccion();

                int nproducc = db.produccion.Count();

                var querynplantas = (from c in db.produccion
                             select c.plantas).ToArray();

                var queryncamas = (from c in db.produccion
                                     select c.camas).ToArray();

                //cargamos el numero de plantas
                int[] pro_plantas = querynplantas;

                ViewBag.producplantas = pro_plantas;

                //cargamos el numero de camas
                int[] pro_camas = queryncamas;

                ViewBag.ncamas = pro_camas;

                double[] acumula_ncamas = new double[nproducc];
                int x = 0; 
                int y = 0;
                double acumuladotot = 0;
                for (x = 0; x< nproducc; x++)
                {
                    if (y == 0 && x == 0)
                    {
                        acumula_ncamas[x] = pro_camas[y];
                    }
                    if(y > 0 && x > 0)
                    {
                        acumula_ncamas[x] = pro_camas[y] + pro_camas[y - 1];
                        
                    }
                    acumuladotot += acumula_ncamas[x];
                    y++;
                }

                ViewBag.facumcamas = acumula_ncamas;


                //multipliacacion xi*f

                double[] med_frec = new double[nproducc];

                int mediap = 0;
                x = 0; y=0;
                double acum_xif=0;
                for(mediap =0; mediap< nproducc; mediap++)
                {
                    med_frec[mediap] = pro_plantas[x] * pro_camas[y];
                    acum_xif += med_frec[mediap];
                    x++;
                    y++;
                }
                ViewBag.medifrec = med_frec;

                //calculo error absoluto

                media = acum_xif/ acumuladotot;
                double[] Error_Absol = new double[nproducc];
                int ea = 0;
                int pro =0;
                double acum_ea = 0; 
                for(ea = 0; ea < nproducc; ea++)
                {
                    Error_Absol[ea] = media - pro_plantas[pro];
                    acum_ea += Error_Absol[ea];
                    pro++;

                }

                ViewBag.cal_ea = Error_Absol;

                return View();
            }
        }
    }
}