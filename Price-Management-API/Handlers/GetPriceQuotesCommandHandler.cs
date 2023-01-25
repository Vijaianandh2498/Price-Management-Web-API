using Newtonsoft.Json;
using Price_Management_Common.Interfaces;
using Price_Management_Common.Requests;
using Price_Management_Common.Responses;
using Price_Management_Common.Utilities;
using System.Reflection.Metadata;

namespace Price_Management_API.Handlers
{
    public class GetPriceQuotesCommandHandler : IWriteHandler<GetPriceQuotesCommandRequest, GetPriceQuotesCommandResponse>
    {
        public GetPriceQuotesCommandHandler()
        {
        }

        public async Task<GetPriceQuotesCommandResponse> WriteOperationAsync(
            GetPriceQuotesCommandRequest getPriceQuotesCommandRequest, CancellationToken cancellationToken)
        {
            var vehicledetails = await GetVechicleDetail(getPriceQuotesCommandRequest).ConfigureAwait(false);

            var filedataobject = JsonConvert.DeserializeObject<Carrier[]>(await GetFileData().ConfigureAwait(false));

            var validCarrierLst = await GetValidCarriersBasedOnVehicle(filedataobject, vehicledetails.Item1).ConfigureAwait(false);


            var pricedetails = await PriceCalculation(getPriceQuotesCommandRequest).ConfigureAwait(false);
            pricedetails += GenenricOperation.GetRoundedPercentageValue(pricedetails, vehicledetails.Item2);

            var calculatedPriceList = await PriceListCalculation(validCarrierLst, pricedetails).ConfigureAwait(false);


            GetPriceQuotesCommandResponse getPriceQuotesCommandResponse = new()
            {
                Delivery_postcode = getPriceQuotesCommandRequest.Delivery_postcode?.Trim(),
                Pickup_postcode = getPriceQuotesCommandRequest.Pickup_postcode?.Trim(),
                Vehicle = vehicledetails.Item1,
                PriceList = calculatedPriceList
            };

            return await Task.FromResult(getPriceQuotesCommandResponse).ConfigureAwait(false);

        }

        private async Task<int> PriceCalculation(GetPriceQuotesCommandRequest getPriceQuotesCommandRequest)
        {
            decimal response = 0;
            if (getPriceQuotesCommandRequest.Delivery_postcode?.Trim().Length > 0 && getPriceQuotesCommandRequest.Pickup_postcode?.Trim().Length > 0)
            {
                var res = GenenricOperation.GetBase36fromString(getPriceQuotesCommandRequest.Pickup_postcode) -
                     GenenricOperation.GetBase36fromString(getPriceQuotesCommandRequest.Delivery_postcode);

                response = res / GenenricOperation.GetLargeIntegerValue();
            }
            return await Task.FromResult(GenenricOperation.ToCeiling(response)).ConfigureAwait(false);
        }

        private async Task<Tuple<string, int>> GetVechicleDetail(GetPriceQuotesCommandRequest getPriceQuotesCommandRequest)
        {
            Tuple<string, int> response = new Tuple<string, int>(string.Empty, 0);
            if (getPriceQuotesCommandRequest.Vehicle?.Trim().Length > 0)
            {
                var vehicle = GenenricOperation.GetEnumValueFromString<
                    Constants.Vehiclepricepercentages>(getPriceQuotesCommandRequest.Vehicle);
                response = new Tuple<string, int>(vehicle.ToString(), (int)vehicle);
            }
            return await Task.FromResult(response).ConfigureAwait(false);
        }

        private async Task<string> GetFileData()
        {
            return await Task.FromResult(GenenricOperation.Readstaticfile("carrier-data.json")).ConfigureAwait(false);
        }

        private async Task<List<Carrier>> GetValidCarriersBasedOnVehicle(Carrier[] carriers, string vehicle)
        {
            bool isvehicleMatched = default;
            List<Carrier> carriersLst = new List<Carrier>();
            var result = carriers?
                 .Where(x => x.services != null).ToList();
            if (result != null)
            {
                foreach (var carrier in result)
                {
                    if (carrier.services != null)
                    {
                        List<Service> services = new();

                        foreach (var service in carrier.services)
                        {
                            if (service.vehicles != null)
                            {
                                foreach (var vehicleAry in service.vehicles.ToList())
                                {
                                    isvehicleMatched = vehicleAry.Equals(vehicle);
                                    if (isvehicleMatched)
                                    {
                                        services.Add(service);
                                    }
                                }
                            }
                        }
                        if (services.Count > 0)
                        {
                            carriersLst.Add(new Carrier
                            {
                                base_price = carrier.base_price,
                                carrier_name = carrier.carrier_name,
                                services = services.ToArray()
                            });
                        }
                    }
                }
            }
            return await Task.FromResult(carriersLst).ConfigureAwait(false);
        }

        private async Task<List<PriceList>> PriceListCalculation(List<Carrier> carriers, int VehiclePricewithMarkup)
        {
            List<PriceList> priceList = new List<PriceList>();
            foreach (var carrier in carriers.Where(x=>x.base_price>0))
            {
                if (carrier != null)
                {
                    var baseprice = carrier.base_price;
                    var carrier_name = carrier.carrier_name;
                    if (carrier.services != null)
                    {
                        foreach (var service in carrier.services)
                        {
                            var servicemarkup = service.markup;
                            var price = VehiclePricewithMarkup + GenenricOperation.GetRoundedPercentageValue(baseprice, servicemarkup);
                            priceList.Add(new PriceList
                            {
                                service = carrier_name,
                                Price = price,
                                delivery_time = service.delivery_time,
                            });
                        }
                    }
                }
            }
            return await Task.FromResult(priceList).ConfigureAwait(false);
        }

    }
}
