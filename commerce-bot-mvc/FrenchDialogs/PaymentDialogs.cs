using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace commerce_bot_mvc.FrenchDialogs
{
    [Serializable]
    public class PaymentDialogs : IDialog<int>
    {
        public async Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(context, this.AfterSelectOption, new string[] { "Stay in this survey", "Get back to where I was" }, "Hello, you're in the survey dialog. Please pick one of these options");
        }

        private async Task AfterSelectOption(IDialogContext context, IAwaitable<string> result)
        {
            if ((await result) == "Get back to where I was")
            {
                await context.PostAsync("Great, back to the original conversation!");
                context.Done(String.Empty); //Finish this dialog
            }
            else
            {
                await context.PostAsync("I'm still on the survey until you tell me to stop");
                PromptDialog.Choice(context, this.AfterSelectOption, new string[] { "Stay in this survey", "Get back to where I was" }, "Hello, you're in the survey dialog. Please pick one of these options");
            }
        }
    }
}
