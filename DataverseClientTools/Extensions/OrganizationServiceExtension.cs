using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace DataverseClientTools.Extensions
{
    public static class OrganizationServiceExtension
    {
        public static async Task<List<ExecuteMultipleResponse>> ExecuteMultipleAsync(
            this IOrganizationServiceAsync organizationService,
            ExecuteMultipleRequest multipleRequest, int entityPerBatch = 1000)
        {
            // maximum entity per request = 1000
            var listOfChunk = multipleRequest.Requests.ChunkBy(entityPerBatch);
            var multipleRequestChunk = new List<ExecuteMultipleRequest>();
            foreach (var chunk in listOfChunk)
            {
                var requestCollection = new OrganizationRequestCollection();
                requestCollection.AddRange(chunk);
                var chunkRequest = new ExecuteMultipleRequest()
                {
                    Settings = multipleRequest.Settings,
                    Requests = requestCollection
                };

                multipleRequestChunk.Add(chunkRequest);
            }

            var result = new List<ExecuteMultipleResponse>();

            foreach (var executeMultipleRequest in multipleRequestChunk)
            {
                var serviceClientResponse =
                    (ExecuteMultipleResponse)await organizationService.ExecuteAsync(executeMultipleRequest);

                result.Add(serviceClientResponse);
            }

            return result;
        }

        public static async Task<List<ExecuteTransactionResponse>> ExecuteTransactionAsync(
            this IOrganizationServiceAsync organizationService,
            ExecuteTransactionRequest transactionRequest, int entityPerBatch = 1000)
        {
            // maximum entity per request = 1000
            var listOfChunk = transactionRequest.Requests.ChunkBy(entityPerBatch);
            var transactionRequestChunk = new List<ExecuteTransactionRequest>();
            foreach (var chunk in listOfChunk)
            {
                var requestCollection = new OrganizationRequestCollection();
                requestCollection.AddRange(chunk);
                var chunkRequest = new ExecuteTransactionRequest()
                {
                    ReturnResponses = true,
                    Requests = requestCollection
                };

                transactionRequestChunk.Add(chunkRequest);
            }

            var result = new List<ExecuteTransactionResponse>();

            foreach (var executeTransactionRequest in transactionRequestChunk)
            {
                var serviceClientResponse =
                    (ExecuteTransactionResponse)await organizationService.ExecuteAsync(executeTransactionRequest);

                result.Add(serviceClientResponse);
            }

            return result;
        }
    }
}