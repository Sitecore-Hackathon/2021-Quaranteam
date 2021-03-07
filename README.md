# Sitecore Hackathon 2021

# Hackathon Submission Entry form

## Team name
⟹ Quaranteam

## Category
⟹ SXA Enhancement

## Description

  - **Module Purpose: Content Reflector for SXA**

    So why did we create this?

    Over the past 3 years, Sitecore has been continually closing the gap with it’s competitors. Sitecore has been one of the leading DXP/CMS platforms, but amongst prospects and even current customers, its reputation was always, “*well, it’s not as easy to use as WordPress, Sitefinity, Kentico, etc.*” Sitecore has added many helpful features for the authors, but there are still major enhancements needed. For example, on a platform like Sitefinity, if a new content type is needed, someone who is not a programmer can literally navigate to a wizard, type in all the fields that are needed, and click a button. **Boom**, they have a new data template and details page created. This is the capability that we created for Sitecore. 

    **More on the scenario**

    Picture a marketer (Suzy) walking into the office on a Monday; as soon as she sits down, her boss runs up to her and says “*We have an emergency! We need a new blog posted immediately because our CEO has an important message to get out.*” This should be simple enough to do,  right? Well, if they were utilizing Sitecore and they didn’t already have a Blog Template or Detail Page already, panic would ensue! Suzy would have had to track down her in-house developer or her partner to code a solution just to meet this need. It would have likely taken a day to code, deploy, test, re-deploy. Suzy’s boss and the CEO would not have been happy! 

    Now, let’s re-imagine this scenario on a future Monday. Suzy utilizes the new helper utility from **Quaranteam**. Suzy clicks a few buttons, fills out some fields, and saves her work within minutes! Behind the scenes, x,y,z is created and the blog is ready to be written and styled!  

	What would have taken hours and days before, has now been cut down to minutes. A marketer can simply create all of their basic content types on their own in minutes, and style each accordingly with pre-configured mark-up.

  **What problem was solved**

   With the base SXA implementation when users want to create a custom type, they have to manaully create templates, renderings, and a rendering variant to display the custom type on the page. ONly then can a FED begin styling the content on the page.
    
   **How does this module solve it**

   Content Reflector for SXA introduces a module that allows users to display a simple Powershell dialog that enables them to:
   - Enter the name for their new type.
    - Select base templates for the new type.
    - Enter field definitions for the new type.

   When the user clicks create, the code behind generates the templates for the new type, creates renderings for the new type, and creates a Scriban rendering variant that iterates through the fields on the new type. With the click of a button, the user is able to create a new SXA template with a rendering that has all the fields already mapped and is ready to be styled by a FED. We were able to achieve this with a Powershell dialog and by extending SXA module builder to simplify the workflow for creation of a custom template and rendering.

    
## Video link
⟹ Provide a video highlighting your Hackathon module submission and provide a link to the video. You can use any video hosting, file share or even upload the video to this repository. _Just remember to update the link below_

⟹ [Replace this Video link](#video-link)

## Pre-requisites and Dependencies

The Content Reflector for SXA requires:
- SXA
- PSE
- Docker configured to run the containers included with the solution.

## Installation instructions

1. In the root of the solution, run Start-hackathon.ps1.
2. Rebuild the solution.
3. Use the Sitecore Installation Wizard to install the ContentReflector package: (#link-to-package)

### Configuration

No custom configuration is required for this package, but it assumes you already have an SXA site.


## Usage instructions

1. Select a node in the Content tree under an SXA site.
2. Right click the node and Select Scripts and New Content Type or click the New Content Type in the ribbon.
3. In the dialog select:
   - The item name.
   - Page templates.
   - Select any base templates. If no base template is selected, we will use the Standard Template as a base.
   - Specify the number of fields you would like on your new type.
   - Click continue.
4. A second dialog displays enabling you to create fields on your new type. For each tab, enter a field name and select a field type.
5. When you're finished entering field information, click Continue.
6. Content Reflector does the following:
   - Creates a new SXA module for the content type that can be reused on any SXA site in a multistie setup.
   - Creates a new template for your type based on the selected standard templates.
   - Adds the selected fields to the new template.
   - Creates Page Content and Page List renderings for the new type.
   - Creates a Sciban rendering variant for the new type that iterates through the fields on the new content type and displays them on the page.
7. Once the wizard completes you have a fully formed content type with a renderign variant that wires up all fields on the page, ready to be styled by a FED using Creative Exchange Live.


## Comments

Thanks to the power and extensibility of SXA, we were able to quickly create an SXA extension that creates a module quickly adds a custom SXA module that adds a custom type to an SXA site with page types and rendering sready to be styled. What used to take hours of developer time can now be accomplished in minutes with a simple dialog.

