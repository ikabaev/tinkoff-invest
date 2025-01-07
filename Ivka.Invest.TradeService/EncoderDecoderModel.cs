using TorchSharp.Modules;
using static TorchSharp.torch;

namespace Ivka.Invest.Trade
{
    internal class EncoderDecoderModel : nn.Module
    {
        private Embedding _encoder_embed;

        protected EncoderDecoderModel(string name) : base(name)
        {
            _encoder_embed = nn.Embedding(1, 2, 3);
        }
        //protected EncoderDecoderModel(string name) : base(name)
        //{
        //    this.encoder_embed = nn.Embedding(input_voc_size, self.input_size)
        //}
    }
}
