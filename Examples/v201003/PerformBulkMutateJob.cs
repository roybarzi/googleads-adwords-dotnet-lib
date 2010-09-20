// Copyright 2010, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// Author: api.anash@gmail.com (Anash P. Oommen)

using com.google.api.adwords.lib;
using com.google.api.adwords.lib.util;
using com.google.api.adwords.v201003;

using System;
using System.Threading;

namespace com.google.api.adwords.examples.v201003 {
  /// <summary>
  /// This code example shows how to add ads and keywords using the
  /// BulkMutateJobService.
  ///
  /// Tags: BulkMutateJobService.mutate
  /// </summary>
  class PerformBulkMutateJob : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This code example shows how to add ads and keywords using the" +
            " BulkMutateJobService.";
      }
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The AdWords user object running the code example.
    /// </param>
    public override void Run(AdWordsUser user) {
      // Get the BulkMutateJobService.
      BulkMutateJobService bmjService = (BulkMutateJobService) user.GetService(
          AdWordsService.v201003.BulkMutateJobService);

      long campaignId = long.Parse(_T("INSERT_CAMPAIGN_ID_HERE"));
      long adGroupId = long.Parse(_T("INSERT_ADGROUP_ID_HERE"));

      // Create an AdGroupAdOperation to add a text ad.
      AdGroupAdOperation adGroupAdOperation = new AdGroupAdOperation();
      adGroupAdOperation.operatorSpecified = true;
      adGroupAdOperation.@operator = Operator.ADD;

      TextAd textAd = new TextAd();
      textAd.headline = "Luxury Cruise to Mars";
      textAd.description1 = "Visit the Red Planet in style.";
      textAd.description2 = "Low-gravity fun for everyone!";
      textAd.displayUrl = "www.example.com";
      textAd.url = "http://www.example.com";

      AdGroupAd adGroupAd = new AdGroupAd();
      adGroupAd.adGroupId = adGroupId;
      adGroupAd.adGroupIdSpecified = true;
      adGroupAd.ad = textAd;

      adGroupAdOperation.operand = adGroupAd;

      // Add that operation into the first stream.
      OperationStream adOpStream = new OperationStream();

      adOpStream.scopingEntityId = new EntityId();
      adOpStream.scopingEntityId.type = EntityIdType.CAMPAIGN_ID;
      adOpStream.scopingEntityId.typeSpecified = true;
      adOpStream.scopingEntityId.value = campaignId;
      adOpStream.scopingEntityId.valueSpecified = true;

      adOpStream.operations = new Operation[] {adGroupAdOperation};

      // Create AdGroupCriterionOperations to add keywords.
      AdGroupCriterionOperation[] adGroupCriterionOperations = new AdGroupCriterionOperation[100];

      for (int i = 0; i < 100; i++) {
        Keyword keyword = new Keyword();
        keyword.text = string.Format("mars cruise {0}", i);
        keyword.matchTypeSpecified = true;
        keyword.matchType = KeywordMatchType.BROAD;

        BiddableAdGroupCriterion criterion = new BiddableAdGroupCriterion();
        criterion.adGroupId = adGroupId;
        criterion.adGroupIdSpecified = true;
        criterion.criterion = keyword;

        AdGroupCriterionOperation adGroupCriterionOperation = new AdGroupCriterionOperation();
        adGroupCriterionOperation.@operator = Operator.ADD;
        adGroupCriterionOperation.operatorSpecified = true;

        adGroupCriterionOperation.operand = criterion;
        adGroupCriterionOperations[i] = adGroupCriterionOperation;
      }

      // Add those operation into the second stream.
      OperationStream keywordOpStream = new OperationStream();

      keywordOpStream.scopingEntityId = new EntityId();
      keywordOpStream.scopingEntityId.type = EntityIdType.CAMPAIGN_ID;
      keywordOpStream.scopingEntityId.typeSpecified = true;
      keywordOpStream.scopingEntityId.value = campaignId;
      keywordOpStream.scopingEntityId.valueSpecified = true;

      keywordOpStream.operations = adGroupCriterionOperations;

      // Create a job.

      // a. Create a bulk job object.
      long bulkJobId = 0;
      BulkMutateJob bulkJob = null;

      bulkJob = new BulkMutateJob();
      bulkJob.numRequestParts = 2;
      bulkJob.numRequestPartsSpecified = true;

      // b. Create a part of the job.

      BulkMutateRequest bulkRequest1 = new BulkMutateRequest();
      bulkRequest1.partIndex = 0;
      bulkRequest1.partIndexSpecified = true;
      bulkRequest1.operationStreams = new OperationStream[] {adOpStream};
      bulkJob.request = bulkRequest1;

      // c. Create job operation.
      JobOperation jobOperation1 = new JobOperation();
      jobOperation1.operatorSpecified = true;
      jobOperation1.@operator = Operator.ADD;
      jobOperation1.operand = bulkJob;

      // d. Call mutate().
      try {
        bulkJob = bmjService.mutate(jobOperation1);
        bulkJobId = bulkJob.id;
      } catch (Exception ex) {
        Console.WriteLine("Failed to create bulk mutate job. Exception says \"{0}\"", ex.Message);
        return;
      }

      // Similarly, create the next part of the job.

      // Note: since we already created a job earlier, this time we modify it.
      bulkJob = new BulkMutateJob();
      bulkJob.idSpecified = true;
      bulkJob.id = bulkJobId;

      BulkMutateRequest bulkRequest2 = new BulkMutateRequest();
      bulkRequest2.partIndex = 1;
      bulkRequest2.partIndexSpecified = true;
      bulkRequest2.operationStreams = new OperationStream[] {keywordOpStream};
      bulkJob.request = bulkRequest2;

      JobOperation jobOperation2 = new JobOperation();
      jobOperation2.operatorSpecified = true;
      jobOperation2.@operator = Operator.SET;
      jobOperation2.operand = bulkJob;

      try {
        bulkJob = bmjService.mutate(jobOperation2);
        bulkJobId = bulkJob.id;
      } catch (Exception ex) {
        Console.WriteLine("Failed to modify bulk mutate job with id = {0}. Exception says \"{1}\"",
            bulkJobId, ex.Message);
        return;
      }

      // Wait for the job to complete.
      bool completed = false;

      while (completed == false) {
        Thread.Sleep(2000);

        BulkMutateJobSelector selector = new BulkMutateJobSelector();
        selector.jobIds = new long[] {bulkJobId};

        try {
          BulkMutateJob[] allJobs = bmjService.get(selector);
          if (allJobs != null && allJobs.Length > 0) {
            if (allJobs[0].status == BasicJobStatus.COMPLETED ||
                allJobs[0].status == BasicJobStatus.FAILED) {
              completed = true;
              bulkJob = allJobs[0];
              break;
            }
          }
        } catch (Exception ex) {
          Console.WriteLine("Failed to fetch bulk mutate job with id = {0}. Exception says \"{1}\"",
              bulkJobId, ex.Message);
          return;
        }
      }

      if (bulkJob.status == BasicJobStatus.COMPLETED) {
        // Retrieve the job parts.
        for (int i = 0; i < bulkJob.numRequestParts; i++) {
          BulkMutateJobSelector selector = new BulkMutateJobSelector();
          selector.jobIds = new long[] {bulkJobId};
          selector.resultPartIndex = i;
          selector.resultPartIndexSpecified = true;

          BulkMutateJob[] allJobParts = bmjService.get(selector);
          foreach (BulkMutateJob jobPart in allJobParts) {
            Console.WriteLine("Part {0}/{1} of job '{2}' has successfully completed.",
                jobPart.result.partIndex + 1, bulkJob.numRequestParts, jobPart.id);
          }
        }
        Console.WriteLine("Job completed successfully!");
      } else {
        Console.WriteLine("Job could not be completed.");
      }
    }
  }
}
