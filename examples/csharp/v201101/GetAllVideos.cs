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
using System.Collections.Generic;
using System.Text;

namespace Google.Api.Ads.AdWords.Examples.CSharp.v201101 {
  /// <summary>
  /// This code example gets all videos. To upload video, see
  /// http://adwords.google.com/support/aw/bin/answer.py?hl=en&amp;answer=39454.
  ///
  /// Tags: MediaService.get
  /// </summary>
  class GetAllVideos : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This code example gets all videos. To upload video, see " +
            "http://adwords.google.com/support/aw/bin/answer.py?hl=en&answer=39454.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main(string[] args) {
      SampleBase codeExample = new GetAllVideos();
      Console.WriteLine(codeExample.Description);
      codeExample.Run(new AdWordsUser());
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The AdWords user object running the code example.
    /// </param>
    public override void Run(AdWordsUser user) {
      // Get the MediaService.
      MediaService mediaService = (MediaService) user.GetService(
          AdWordsService.v201101.MediaService);

      Selector selector = new Selector();
      selector.fields = new string[] {"MediaId", "Type"};

      // Create a filter.
      Predicate predicate = new Predicate();
      predicate.@operator = PredicateOperator.EQUALS;
      predicate.field = "Type";
      predicate.values = new string[] {MediaMediaType.VIDEO.ToString()};

      selector.predicates = new Predicate[] {predicate};

      try {
        // Get all videos.
        MediaPage page = mediaService.get(selector);

        // Display videos.
        if (page != null && page.entries != null && page.entries.Length > 0) {
          foreach (Video video in page.entries) {
            Console.WriteLine("Video with id \"{0}\" and name \"{1}\" was found.",
                video.mediaId, video.name);
          }
        } else {
          Console.WriteLine("No images were found.");
        }
      } catch (Exception ex) {
        Console.WriteLine("Failed to get all images. Exception says \"{0}\"", ex.Message);
      }
    }
  }
}