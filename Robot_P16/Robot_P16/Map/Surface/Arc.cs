using System;
using Microsoft.SPOT;

namespace Robot_P16.Map.Surface
{
    class Arc : ElementSurface
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

        public override bool Appartient(PointOriente p)
        {
            double rel_x = p.x - centre.x;
            double rel_y = p.y - centre.y;

            if (rel_x == 0 && rel_y == 0)
            {
                return distance == 0;
            }
            else
            {
                bool horsCercle1 = (rel_x * rel_x + rel_y * rel_y) >= distance;
                bool dansCercle2 = (rel_x * rel_x + rel_y * rel_y) <= distance + largeur;
                bool dansCadrant = (System.Math.Atan(rel_y / rel_x) >= p.theta && System.Math.Atan(rel_y / rel_x) <= p.theta + angle);
                return horsCercle1 && dansCercle2 && dansCadrant;
            }
        }
    }
}
