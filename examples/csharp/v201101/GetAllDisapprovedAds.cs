// Copyright 2011, Google Inc. All Rights Reserved.
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

using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.v201101;

using System;

namespace Google.Api.Ads.AdWords.Examples.CSharp.v201101 {
  /// <summary>
  /// This code example retrieves all the disapproved ads in a given campaign.
  ///
  /// Tags: AdGroupAdService.get
  /// </summary>
  class GetAllDisapprovedAds : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This code example retrieves all the disapproved ads in a given campaign.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main(string[] args) {
      SampleBase codeExample = new GetAllDisapprovedAds();
      Console.WriteLine(codeExample.Description);
      codeExample.Run(new AdWordsUser());
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The AdWords user object running the code example.
    /// </param>
    public override void Run(AdWordsUser user) {
      // Get the AdGroupAdService.
      AdGroupAdService service =
          (AdGroupAdService) user.GetService(AdWordsService.v201101.AdGroupAdService);

      long campaignId = long.Parse(_T("INSERT_CAMPAIGN_ID_HERE"));

      // Create a selector.
      Selector selector = new Selector();
      selector.fields = new string[] {"Id", "CreativeApprovalStatus", "DisapprovalReasons"};

      // Create a filter.
      Predicate predicate = new Predicate();
      predicate.@operator = PredicateOperator.EQUALS;
      predicate.field = "CampaignId";
      predicate.values = new string[] {campaignId.ToString()};

      selector.predicates = new Predicate[] {predicate};

      try {
        AdGroupAdPage page = service.get(selector);

        if (page != null && page.entries != null) {
          foreach (AdGroupAd tempAdGroupAd in page.entries) {
            if (tempAdGroupAd.ad.approvalStatus == AdApprovalStatus.DISAPPROVED) {
              Console.WriteLine("Ad id {0} has been disapproved for the following reason(s):",
                  tempAdGroupAd.ad.id);
              foreach (string reason in tempAdGroupAd.ad.disapprovalReasons) {
                Console.WriteLine("    {0}", reason);
              }
            }
          }
        }
      } catch (Exception ex) {
        Console.WriteLine("Failed to get Ad(s). Exception says \"{0}\"", ex.Message);
      }
    }
  }
}