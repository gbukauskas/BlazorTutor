using Microsoft.FluentUI.AspNetCore.Components;
using System.Runtime.Intrinsics.Arm;

namespace FluentUiNorthWind.Components.Shared
{
    public static class NorthWindIcons
    {
        public class CampingTent : Icon {
            public CampingTent() : base("CampingTent", IconVariant.Regular, IconSize.Size20,
                "<svg xmlns=\"http://www.w3.org/2000/svg\" fill=\"none\" viewBox=\"0 0 14 14\" id=\"Camping-Tent--Streamline-Core\" height=\"14\" width=\"14\"><desc>Camping Tent Streamline Icon: https://streamlinehq.com</desc><g id=\"camping-tent--outdoor-recreation-camping-tent-teepee-tipi-travel-places\"><path id=\"Subtract\" fill=\"#000000\" fill-rule=\"evenodd\" d=\"M4.66911 0.130751c0.34668 -0.2266781 0.81148 -0.12939394 1.03816 0.21729L7.00001 2.32518 8.29276 0.348041c0.22667 -0.34668394 0.69148 -0.4439681 1.03816 -0.21729 0.34669 0.226678 0.44397 0.69148 0.21729 1.038169L7.8961 3.69567l5.9732 9.13543c0.1507 0.2305 0.1629 0.5251 0.0319 0.7673 -0.131 0.2422 -0.3843 0.3931 -0.6597 0.3931H8v-3.0192c0 -0.5523 -0.44772 -0.99999 -1 -0.99999S6 10.42 6 10.9723v3.0192H0.758485c-0.275396 0 -0.52864 -0.1509 -0.6596698 -0.3931 -0.1310302 -0.2422 -0.1187676 -0.5368 0.0319428 -0.7673L6.10392 3.69567l-1.6521 -2.52675c-0.22668 -0.346689 -0.1294 -0.811491 0.21729 -1.038169Z\" clip-rule=\"evenodd\" stroke-width=\"1\"></path></g></svg>"
                )
            { }
        }
        public class OpenBook : Icon
        {
            public OpenBook() : base("OpenBook", IconVariant.Regular, IconSize.Size20,
                "<svg xmlns =\"http://www.w3.org/2000/svg\" fill=\"none\" viewBox=\"0 0 14 14\" id=\"Open-Book--Streamline-Core\" height=\"14\" width=\"14\"><desc>Open Book Streamline Icon: https://streamlinehq.com</desc><g id =\"open-book--content-books-book-open\"><path id =\"Subtract\" fill=\"#000000\" fill-rule=\"evenodd\" d=\"M6.375 1.653C5.386 1.099 3.536 0.42 1.496 0.179 0.674 0.082 0 0.76 0 1.588v8c0 0.829 0.677 1.489 1.492 1.637 1.84 0.334 3.371 1.216 4.348 1.914 0.164 0.117 0.345 0.205 0.535 0.266V1.653Zm1.25 11.752c0.19 -0.06 0.37 -0.149 0.534 -0.265 0.977 -0.698 2.508 -1.581 4.349 -1.915 0.815 -0.148 1.492 -0.808 1.492 -1.637v-8C14 0.76 13.326 0.082 12.504 0.18c-2.04 0.242 -3.89 0.92 -4.879 1.474v11.752Z\" clip-rule=\"evenodd\" stroke-width=\"1\"></path></g></svg >"
                )
            { }
        }
        public class BusinessCard : Icon
        {
            public BusinessCard() : base("BusinessCard", IconVariant.Regular, IconSize.Size20,
                "<svg xmlns =\"http://www.w3.org/2000/svg\" fill=\"none\" viewBox=\"0 0 14 14\" id=\"Business-Card--Streamline-Core\" height=\"14\" width=\"14\"><desc>Business Card Streamline Icon: https://streamlinehq.com</desc>< g id =\"business-card--name-card-business-information-money-payment\"><path id =\"Subtract\" fill=\"#000000\" fill-rule=\"evenodd\" d=\"M1.5 2C0.671573 2 0 2.67157 0 3.5v7c0 0.8284 0.671573 1.5 1.5 1.5h11c0.8284 0 1.5 -0.6716 1.5 -1.5v-7c0 -0.82843 -0.6716 -1.5 -1.5 -1.5h-11Zm2.77432 2.875c-1.1736 0 -2.125 0.95139 -2.125 2.125 0 1.1736 0.9514 2.125 2.125 2.125 1.17361 0 2.125 -0.9514 2.125 -2.125 0 -1.17361 -0.95139 -2.125 -2.125 -2.125Zm3.82636 0.625c0 -0.34518 0.27982 -0.625 0.625 -0.625h2.50002c0.3452 0 0.625 0.27982 0.625 0.625s-0.2798 0.625 -0.625 0.625H8.72568c-0.34518 0 -0.625 -0.27982 -0.625 -0.625Zm0.625 2.375c-0.34518 0 -0.625 0.27982 -0.625 0.625s0.27982 0.625 0.625 0.625h2.50002c0.3452 0 0.625 -0.27982 0.625 -0.625s-0.2798 -0.625 -0.625 -0.625H8.72568Z\" clip-rule=\"evenodd\" stroke-width=\"1\"></path></g></svg>"
                )
            { }
        }
        public class OfficeWorker : Icon
        {
            public OfficeWorker() : base("OfficeWorker", IconVariant.Regular, IconSize.Size20,
                "<svg xmlns=\"http://www.w3.org/2000/svg\" fill=\"none\" viewBox=\"0 0 14 14\" id=\"Office-Worker--Streamline-Core\" height=\"14\" width=\"14\"><desc>Office Worker Streamline Icon: https://streamlinehq.com</desc>< g id =\"office-worker--office-worker-human-resources\"><path id =\"Union\" fill=\"#000000\" fill-rule=\"evenodd\" d=\"M3.5 4a2 2 0 1 0 0 -4 2 2 0 0 0 0 4Zm2.045 6v-0.03c0 -0.99 0.547 -1.852 1.355 -2.302A3.5 3.5 0 0 0 0 8.5v1a0.5 0.5 0 0 0 0.5 0.5h1l0.445 3.562a0.5 0.5 0 0 0 0.496 0.438H4.56a0.5 0.5 0 0 0 0.496 -0.438L5.5 10h0.045Zm4.382 -2.313a0.25 0.25 0 0 0 -0.25 0.25v0.651h1.413v-0.65a0.25 0.25 0 0 0 -0.25 -0.25h-0.913Zm-1.75 0.25v0.651c-0.763 0 -1.382 0.62 -1.382 1.383v2.647c0 0.763 0.619 1.382 1.382 1.382h4.412c0.763 0 1.382 -0.619 1.382 -1.382V9.97c0 -0.763 -0.618 -1.382 -1.381 -1.382v-0.65a1.75 1.75 0 0 0 -1.75 -1.75h-0.913a1.75 1.75 0 0 0 -1.75 1.75Z\" clip-rule=\"evenodd\" stroke-width=\"1\"></path></g></svg>"
                )
            { }
        }
    }
}
