﻿using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace NetMapper.Services
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver s, IReadonlyDependencyResolver r)
        {
            // Json Settings Store
            s.Register<IStore<AppSettingsModel>>(() => new JsonStore<AppSettingsModel>("AppSettings.json"));

            // Json Drive List Store
            s.Register<IStore<List<MappingModel>>>(() => new JsonStore<List<MappingModel>>("NetDriveSettings.json"));           

            // Notification
            s.RegisterConstant(new NotificationService());


            // DriveListManager
            s.RegisterConstant(new DriveListService(
                r.GetRequiredService<IStore<List<MappingModel>>>()));

            // StateResolver
            s.RegisterConstant(new DriveConnectService(
                r.GetRequiredService<DriveListService>(),
                r.GetRequiredService<NotificationService>()));

            // StateGenerator
            s.RegisterConstant(new DrivePoolingService(
                r.GetRequiredService<DriveListService>(),
                r.GetRequiredService<DriveConnectService>()));


        }
        public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver)
        {

            var service = resolver.GetService<TService>() ??
                throw new InvalidOperationException($"Failed to resolve object of type {typeof(TService)}"); // throw error with detailed description

            // design mode bypass      
            return service; // return instance if not null
        }

    }



}
