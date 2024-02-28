using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class PromptInstructions
    {
        public const string Instructions = "Prompt:"
                                            + "  I will provide you an outline for a blog post. I want you to write a blog post according to this outline and my instructions. "
                                            + "  Here are the instructions:"
                                            + "  - The output should be in english language. "
                                            + "  - Make it at least 1,500 words long using # headings # and # sub-headings #. "
                                            + "  - Make sure to write the blog post according to the outline I will provide you, as well as according to SEO standards and Google's E-E-A-T (Expertise, Experience, Authority, Transparency). "
                                            + "  - Write only high-quality content"
                                            + "  - Make the content highly detailed and in-depth"
                                            + "  - Make sure your output has the highest E-E-A-T degree possible. "
                                            + "  - Write unique text and sentences, instead of copying and pasting from different sources. "
                                            + "  - Write with a high degree of perplexity and burstiness, but use common words, so that your output is easy to understand. "
                                            + "  - Make it human-like and add a little bit of humour. "
                                            + "  - Avoid wordiness."
                                            + "  - Use text formatting ( H1, H2 and H3 tags). "
                                            + "  - Bold the Headings and Sub-headings using Markdown formatting.  "
                                            + "  - After conclusion, please insert FAQs section with a minimum of 3 questions and answers related to the blog post. "
                                            + "  - Make sure content you write can easily pass AI detection. "
                                            + "  - If possible, try to bold important keywords. "
                                            + "  - Make sure the blog post is plagiarism-free. "
                                            + "  - Try to use contractions, idioms, transitional phrases, interjections, dangling modifiers, and colloquialisms, and avoid repetitive phrases and unnatural sentence structures. "
                                            + "  - Always use a combination of paragraphs, lists, and tables for a better reader experience. "
                                            + "  - Each paragraph you write should be at least 500 words long. Try to add real-life examples. "
                                            + "  - Try to add as much HTML (bullet point lists, tables) as it is possible, because it improves the SEO. "
                                            + "  - Start with writing the meta title and meta description for a blog post and then move on to writing an actual blog post. "
                                            + "  - Before you write your response, add this custom message:"
                                            + "  Here is the outline:";

        public const string Instruction2 = "You are a teacher and you explain any topic with pictorial presentation to the student. "
                                            + " - Ensure that the output is presented in clear and simple English language, making it easy to understand and remember."
                                            + " - create content similar to Wikipedia formate"
                                            + " - create placeholder for images which will be similar to Wikipedia"
                                            + " - Make it at most 1,500 words long using # headings # and # sub-headings #."
                                            + " - Use text formatting ( H1, H2, H3 , P, Label, Img tags)."
                                            + " - Generate proper formated mathematical equation for better display."
                                            + " - Bold the Headings and Sub-headings using Markdown formatting."
                                            + " - Make sure content you write can easily undestandable by 15 years student."
                                            + " - If possible, try to bold important words."
                                            + " - If possible, try to under line defination."
                                            + " - Wherever an image is required to explain the content visually for the student or reader add a image tags after that each paragraph."
                                            + " - Ensure that the image tag is added appropriately within the corresponding content paragraph."
                                            + " - Ensure that the image tag must have the \"alt\" attribute."
                                            + " - Please add a one-line summary describing the content of the image in the 'alt' attribute of the image tag corresponding to the content paragraph."
                                            + " - Ensure that if any content is generate whcih contains any mathematical notation thant it should be generate in MathJax library supported and config=TeX-MML-AM_CHTML."
                                            + " - Try to add as much HTML (bullet point lists, tables) as it is possible.";
    }
}
