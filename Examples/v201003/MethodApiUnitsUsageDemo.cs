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

using System;
using System.Collections;
using System.Collections.Generic;

namespace com.google.api.adwords.examples.v201003 {
  /// <summary>
  /// This code example displays API method usage for this month for all methods provided
  /// by the AdWords API. Note that this data is not in real time and is refreshed
  /// every few hours.
  /// </summary>
  class MethodApiUnitsUsageDemo : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This code example displays API method usage for this month for all methods" +
            " provided by the AdWords API. Note that this data is not in real time and is" +
            " refreshed every few hours.";
      }
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The AdWords user object running the code example.
    /// </param>
    public override void Run(AdWordsUser user) {
      user.ResetUnits();
      List<MethodQuotaUsage> methodQuotaUsage = UnitsUtilities.GetMethodQuotaUsage(user,
          DateTime.Now.AddMonths(-1), DateTime.Now);

      foreach (MethodQuotaUsage usage in methodQuotaUsage) {
        Console.WriteLine("{0,-50} - {1}", usage.serviceName + "." + usage.methodName,
            usage.units);
      }
      Console.WriteLine("\nTotal Quota unit cost for this run: {0}.\n", user.GetUnits());
    }
  }
}
