﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Conclusion;
using Gx.Links;

namespace Gx.Rs.Api
{
    public class PlaceDescriptionsState : GedcomxApplicationState<Gedcomx>
    {
        internal PlaceDescriptionsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new PlaceDescriptionsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        public List<PlaceDescription> PlaceDescriptions
        {
            get
            {
                return Entity == null ? null : Entity.Places;
            }
        }

        public PlaceDescriptionState AddPlaceDescription(PlaceDescription place, params StateTransitionOption[] options)
        {
            Gedcomx entity = new Gedcomx();
            entity.AddPlace(place);
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(GetSelfUri(), Method.POST);
            return this.stateFactory.NewPlaceDescriptionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public CollectionState ReadCollection(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.COLLECTION);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
