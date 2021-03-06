﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace QuickNavigate.Test
{
    [TestClass]
    public class SearchUtilTest
    {
        [TestMethod]
        public void SimpleSearchMatch_WholeWord_MatchCase()
        {
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "flash", true));
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "flash.display", true));
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "flash.display.Sprite", true));
            Assert.IsFalse(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "display", true));
            Assert.IsFalse(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "display.Sprite", true));
            Assert.IsFalse(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "Sprite", true));
            Assert.IsFalse(SearchUtil.SimpleSearchMatch("flash.display.DisplayObject", "Object", true));
        }

        [TestMethod]
        public void SimpleSearchMatch_NoWholeWord_MatchCase()
        {
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "flash", false));
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "flash.display", false));
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "flash.display.Sprite", false));
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "display", false));
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "display.Sprite", false));
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "Sprite", false));
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.DisplayObject", "Object", false));
            Assert.IsFalse(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "flash.display.sprite", false));
            Assert.IsFalse(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "display.sprite", false));
            Assert.IsFalse(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "sprite", false));
            Assert.IsFalse(SearchUtil.SimpleSearchMatch("flash.display.DisplayObject", "object", false));
            Assert.IsFalse(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "flash.Sprite", false));
        }

        [TestMethod]
        public void SimpleSearchMatch_NoWholeWord_NoCase()
        {
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "flash.display.sprite", false, true));
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "display.sprite", false, true));
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.Sprite", "sprite", false, true));
            Assert.IsTrue(SearchUtil.SimpleSearchMatch("flash.display.DisplayObject", "object", false, true));
        }

        [TestMethod]
        public void GetParts_MatchCase()
        {
            List<string> parts = SearchUtil.GetParts("DisplayObjectContainer", false);
            Assert.AreEqual(parts[0], "Display");
            Assert.AreEqual(parts[1], "Object");
            Assert.AreEqual(parts[2], "Container");
        }

        [TestMethod]
        public void GetParts_NoCase()
        {
            List<string> parts = SearchUtil.GetParts("DisplayObjectContainer", true);
            Assert.AreEqual(parts[0], "display");
            Assert.AreEqual(parts[1], "object");
            Assert.AreEqual(parts[2], "container");
        }

        [TestMethod]
        public void AdvancedSearchMatch_MathCase()
        {
            Assert.IsTrue(SearchUtil.AdvancedSearchMatch("DisplayObjectContainer", "DOC", false));
            Assert.IsTrue(SearchUtil.AdvancedSearchMatch("DisplayObjectContainer", "DisplayOC", false));
            Assert.IsFalse(SearchUtil.AdvancedSearchMatch("DisplayObjectContainer", "doc", false));
            Assert.IsFalse(SearchUtil.AdvancedSearchMatch("DisplayObjectContainer", "DISPLAYOC", false));
        }

        [TestMethod]
        public void AdvancedSearchMatch_NoCase()
        {
            Assert.IsTrue(SearchUtil.AdvancedSearchMatch("DisplayObjectContainer", "DOC", true));
            Assert.IsTrue(SearchUtil.AdvancedSearchMatch("DisplayObjectContainer", "DisObjCont", true));
            Assert.IsTrue(SearchUtil.AdvancedSearchMatch("DisplayObjectContainer", "doc", true));
            Assert.IsTrue(SearchUtil.AdvancedSearchMatch("DisplayObjectContainer", "displayoc", true));
            Assert.IsTrue(SearchUtil.AdvancedSearchMatch("DisplayObjectContainer", "displayocontainer", true));
        }

        [TestMethod]
        public void Search_Type_NoWholeWord_MatchCase()
        {
            List<string> source = new List<string>()
            {
                "flash.display.DisplayObject",
                "flash.display.DisplayObjectContainer",
                "flash.display.Shape",
                "flash.display.Sprite",
                "flash.display.MovieClip"
            };
            List<string> matches = SearchUtil.Matches(source, "D", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 2);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObject"));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "d", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 0);
            matches = SearchUtil.Matches(source, "Display", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 2);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObject"));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "display", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 0);
            matches = SearchUtil.Matches(source, "DisplayObject", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 2);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObject"));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "displayobject", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 0);
            matches = SearchUtil.Matches(source, "Object", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 2);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObject"));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "object", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 0);
            matches = SearchUtil.Matches(source, "DO", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 2);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObject"));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "do", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 0);
            matches = SearchUtil.Matches(source, "DOC", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 1);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "doc", ".", 100, false, true);
            Assert.AreEqual(matches.Count, 0);
        }

        [TestMethod]
        public void Search_Type_NoWholeWord_NoCase()
        {
            List<string> source = new List<string>()
            {
                "flash.display.DisplayObject",
                "flash.display.DisplayObjectContainer",
                "flash.display.Shape",
                "flash.display.Sprite",
                "flash.display.MovieClip"
            };
            List<string> matches = SearchUtil.Matches(source, "d", ".", 100, false, false);
            Assert.AreEqual(matches.Count, 2);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObject"));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "display", ".", 100, false, false);
            Assert.AreEqual(matches.Count, 2);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObject"));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "displayobject", ".", 100, false, false);
            Assert.AreEqual(matches.Count, 2);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObject"));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "object", ".", 100, false, false);
            Assert.AreEqual(matches.Count, 2);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObject"));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "do", ".", 100, false, false);
            Assert.AreEqual(matches.Count, 2);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObject"));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "doc", ".", 100, false, false);
            Assert.AreEqual(matches.Count, 1);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "flash.", ".", 100, false, false);
            Assert.AreEqual(matches.Count, source.Count);
            foreach (string m in matches) Assert.IsTrue(source.Contains(m));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "display.", ".", 100, false, false);
            Assert.AreEqual(matches.Count, source.Count);
            foreach (string m in matches) Assert.IsTrue(source.Contains(m));
        }

        [TestMethod]
        public void Search_Type_WholeWord_MatchCase()
        {
            List<string> source = new List<string>()
            {
                "flash.display.DisplayObject",
                "flash.display.DisplayObjectContainer",
                "flash.display.Shape",
                "flash.display.Sprite",
                "flash.display.MovieClip"
            };
            List<string> matches = SearchUtil.Matches(source, "flash.", ".", 100, true, true);
            Assert.AreEqual(matches.Count, source.Count);
            matches = SearchUtil.Matches(source, "FLASH.", ".", 100, true, true);
            Assert.AreEqual(matches.Count, 0);
            matches = SearchUtil.Matches(source, "flash.display", ".", 100, true, true);
            Assert.AreEqual(matches.Count, source.Count);
            matches = SearchUtil.Matches(source, "FLASH.DISPLAY", ".", 100, true, true);
            Assert.AreEqual(matches.Count, 0);
            matches = SearchUtil.Matches(source, "flash.display.Display", ".", 100, true, true);
            Assert.AreEqual(matches.Count, 2);
            Assert.IsTrue(matches.Contains("flash.display.DisplayObject"));
            Assert.IsTrue(matches.Contains("flash.display.DisplayObjectContainer"));
            matches = SearchUtil.Matches(source, "flash.display.display", ".", 100, true, true);
            Assert.AreEqual(matches.Count, 0);
        }

        [TestMethod]
        public void Search_Type_WholeWord_NoCase()
        {
            List<string> source = new List<string>()
            {
                "flash.display.DisplayObject",
                "flash.display.DisplayObjectContainer",
                "flash.display.Shape",
                "flash.display.Sprite",
                "flash.display.MovieClip"
            };
            List<string> matches = SearchUtil.Matches(source, "FLASH.", ".", 100, true, false);
            Assert.AreEqual(matches.Count, source.Count);
            matches = SearchUtil.Matches(source, "FLASH.DISPLAY", ".", 100, true, false);
            Assert.AreEqual(matches.Count, source.Count);
            matches = SearchUtil.Matches(source, "FLASH.DISPLAY.Display", ".", 100, true, false);
            Assert.AreEqual(matches.Count, 2);
            matches = SearchUtil.Matches(source, "flash.display.display", ".", 100, true, false);
            Assert.AreEqual(matches.Count, 2);
            matches = SearchUtil.Matches(source, "Object", ".", 100, true, false);
            Assert.AreEqual(matches.Count, 0);
        }

        [TestMethod]
        public void Sort_Type_NoCase()
        {
            List<string> matches = new List<string>()
            {
                "flash.display.DisplayObject",
                "flash.display.DisplayObjectContainer",
                "flash.display.Shape",
                "flash.display.Sprite",
                "flash.display.MovieClip"
            };
            string[] result = SearchUtil.Sort(matches, "display", ".", true);
            Assert.AreEqual(result[0], "flash.display.DisplayObject");
            Assert.AreEqual(result[1], "flash.display.DisplayObjectContainer");
            Assert.AreEqual(result[2], "flash.display.Shape");
            Assert.AreEqual(result[3], "flash.display.Sprite");
            Assert.AreEqual(result[4], "flash.display.MovieClip");
            matches = new List<string>()
            {
                "org.flashdevelop.tests.quicknavigate.display.DisplayObject",
                "flash.display.DisplayObject",
                "ru.crazypanda.display.DisplayObject",
            };
            result = SearchUtil.Sort(matches, "display", ".", true);
            Assert.AreEqual(result[0], "flash.display.DisplayObject");
            Assert.AreEqual(result[1], "ru.crazypanda.display.DisplayObject");
            Assert.AreEqual(result[2], "org.flashdevelop.tests.quicknavigate.display.DisplayObject");
        }
    }
}