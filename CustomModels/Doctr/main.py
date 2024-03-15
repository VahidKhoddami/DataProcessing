from doctr.io import DocumentFile
from doctr.models import ocr_predictor
from fastapi import FastAPI, File, UploadFile
from fastapi.middleware.cors import CORSMiddleware
from PIL import Image
from io import BytesIO
import os

app = FastAPI()

# Add CORS middleware to allow all origins
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # TODO: Set this to the frontend URL in production
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.post("/convert/")
async def convert_to_text_doctr(file: UploadFile = File(...)):
    try:
        # Read image bytes
        image_bytes = await file.read()
        
        
        # Generate a unique filename
        filename = f"temp_{file.filename}"
        # Save the image
        save_image(image_bytes, filename)

        model = ocr_predictor(pretrained=True)
        multi_img_doc = DocumentFile.from_images([filename])
        result = model(multi_img_doc)
        text= result.render()
         
         # Delete the temporary image after processing
        os.remove(filename)
        
        return {"text" : text}  
        
    except Exception as e:
        return {"error" : str(e)}

# Function to save uploaded image
def save_image(file_bytes, filename):
    with open(filename, "wb") as buffer:
        buffer.write(file_bytes)
        

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=80)