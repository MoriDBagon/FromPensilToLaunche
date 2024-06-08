from flask import Flask, request, jsonify
from PIL import Image
from io import BytesIO
from flask_cors import CORS
from diffusers import AutoPipelineForText2Image, DEISMultistepScheduler, AutoPipelineForImage2Image
from diffusers.utils import load_image
import torch
import base64

app = Flask(__name__)
CORS(app)

model_id = 'lykon/AnyLoRa'
device = "cuda" if torch.cuda.is_available() else "cpu"
pipe = AutoPipelineForText2Image.from_pretrained(
    model_id, 
    torch_dtype=torch.float16, 
    variant="fp16", 
    safety_checker = None, 
    requires_safety_checker = False)

pipe.scheduler = DEISMultistepScheduler.from_config(pipe.scheduler.config)
pipe = pipe.to(device)
lora_path = "bb_trained-06.safetensors" #only one tensor , not folder
pipe.load_lora_weights('.' , weight_name = lora_path)

pipe2 = AutoPipelineForImage2Image.from_pretrained(
    model_id,
    torch_dtype = torch.float16,
    variant = "fp16",
    safety_checker = None, 
    requires_safety_checker = False
)

pipe2.scheduler = DEISMultistepScheduler.from_config(pipe.scheduler.config)
pipe2 = pipe2.to(device)
lora_path = "bb_trained-06.safetensors" #only one tensor , not folder
pipe2.load_lora_weights('.' , weight_name = lora_path)

@app.route('/generate_image', methods = ['POST'])
def generate_image():
    data = request.json
    app.logger.info('Received imageUrl: %s', data)
    if 'imageUrl' in data:
        image_url = data['imageUrl']
        image_url = str(image_url)
        image1 = load_image(image_url)
        app.logger.info('Received imageUrl: %s', image1)
    else:
        image_url = None

    prompt = data['prompt']

    if image_url is None:
        output_base64 = text2img(prompt)
    else:
        output_base64 = img2img(prompt, image1)

    return jsonify({'image' : output_base64})

def text2img(prompt):
    image = pipe(
        prompt = prompt,
        width=512,
        height=512,
    ).images[0]
    img_io = BytesIO()
    image.save('image.png')
    image.save(img_io,format = 'PNG')
    img_io.seek(0)
    image_output = base64.b64encode(img_io.getvalue()).decode('utf-8')
    return image_output

def img2img(prompt,image1):
    image = pipe2(
        prompt = prompt,
        image = image1,
        width=512,
        height=512,
    ).images[0]
    img_io = BytesIO()
    image.save('image.png')
    image.save(img_io,format = 'PNG')
    img_io.seek(0)
    image_output = base64.b64encode(img_io.getvalue()).decode('utf-8')
    return image_output

if __name__ == '__main__':
    app.run(host = '0.0.0.0', port = 5000, debug = True)