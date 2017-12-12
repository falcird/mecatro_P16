using System;
using Microsoft.SPOT;

namespace Robot_P16.Map
{
    class PointOriente
    {
        public double x;
        public double y;
        public double theta;

        public PointOriente(double x, double y, double theta)
        {
            this.x = x;
            this.y = y;
            this.theta = theta;
        }
    }


    class ElementSurface
    {
        class Rectangle
        {
            private PointOriente origine; // origine du rectangle
            private double largeur;
            private double longueur;

            public Rectangle(PointOriente p, double la, double lo)
            {
                origine = p;
                largeur = la;
                longueur = lo;
            }

            public bool Appartient(PointOriente p)
            {
                double rel_x = p.x - origine.x;
                double rel_y = p.y - origine.y;
                double proj_rel_1 = System.Math.Cos(origine.theta) * rel_x + System.Math.Sin(origine.theta) * rel_y;
                double proj_rel_2 = -System.Math.Sin(origine.theta) * rel_x + System.Math.Cos(origine.theta) * rel_y;
                if ((0 <= proj_rel_1 && proj_rel_1 <= longueur) && (-largeur/2 <= proj_rel_2 && proj_rel_2 <= largeur/2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            class Arc
            {
            private PointOriente centre; // centre du cercle dont on veut extraire l'arc
            private double distance;
            private double largeur;
            private double angle;

            public Arc(PointOriente p, double l, double d, double a)
            {
                centre = p;
                largeur = l;
                distance = d;
                angle = a;
            }

            public bool Appartient(PointOriente p)
            {
                double rel_x = p.x - centre.x;
                double rel_y = p.y - centre.y;
                double proj_rel_1 = System.Math.Cos(origine.theta) * rel_x + System.Math.Sin(origine.theta) * rel_y;
                double proj_rel_2 = -System.Math.Sin(origine.theta) * rel_x + System.Math.Cos(origine.theta) * rel_y;
                if ((0 <= proj_rel_1 && proj_rel_1 <= longueur) && (-largeur/2 <= proj_rel_2 && proj_rel_2 <= largeur/2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }



}