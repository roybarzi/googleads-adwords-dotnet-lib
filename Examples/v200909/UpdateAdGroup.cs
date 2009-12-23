// Copyright 2009, Google Inc. All Rights Reserved.
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
using com.google.api.adwords.v200909;

using System;
using System.IO;
using System.Net;

namespace com.google.api.adwords.samples.v200909 {
  /// <summary>
  /// This example illustrates how to update an ad group, setting its status to
  /// 'PAUSED'. To create an ad group, run AddAdGroup.cs.
  /// </summary>
  class UpdateAdGroup : SampleBase {
    /// <summary>
    /// Returns a description about the sample code.
    /// </summary>
    public override string Description {
      get {
        return "This example illustrates how to update an ad group, setting its status to " +
            "'PAUSED'. To create an ad group, run AddAdGroup.cs";
      }
    }

    /// <summary>
    /// Run the sample code.
    /// </summary>
    /// <param name="user">The AdWords user object running the sample.
    /// </param>
    public override void Run(AdWordsUser user) {
      // Get the AdGroupService.
      AdGroupService adGroupService =
          (AdGroupService) user.GetService(AdWordsService.v200909.AdGroupService);

      long adGroupId = long.Parse(_T("INSERT_AD_GROUP_ID_HERE"));

      AdGroup adGroup = new AdGroup();
      adGroup.statusSpecified = true;
      adGroup.status = AdGroupStatus.PAUSED;
      adGroup.idSpecified = true;
      adGroup.id = adGroupId;

      AdGroupOperation operation = new AdGroupOperation();
      operation.operatorSpecified = true;
      operation.@operator = Operator.SET;
      operation.operand = adGroup;

      try {
        AdGroupReturnValue retVal = adGroupService.mutate(new AdGroupOperation[] {operation});
        if (retVal != null && retVal.value != null) {
          foreach (AdGroup adGroupValue in retVal.value) {
            Console.WriteLine("Ad group with id = '{0}' was successfully updated.",
                adGroupValue.id);
          }
        }
      } catch (Exception ex) {
        Console.WriteLine("Failed to update ad group(s). Exception says \"{0}\"", ex.Message);
      }
    }
  }
}